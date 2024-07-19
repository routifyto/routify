using System.Text;
using Routify.Core.Extensions;
using Routify.Core.Utils;
using Routify.Data.Models;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Utils;

namespace Routify.Gateway.Providers;

internal abstract class CompletionProviderBase<TInput, TOutput>
    : ICompletionProvider
    where TInput : ICompletionInput
    where TOutput : ICompletionOutput
{
    public abstract string Id { get; }
    public abstract Dictionary<string, CompletionModel> Models { get; }

    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request,
        CancellationToken cancellationToken)
    {
        var startedAt = DateTime.UtcNow;
        var input = PrepareInput(request);
        var requestUrl = PrepareRequestUrl(request);
        var requestJson = RoutifyJsonSerializer.Serialize(input);
        
        var isCacheEnabled = request.Context.Route.CacheConfig?.Enabled == true;
        var (response, isCacheHit) = await GetResponseAsync(request, requestUrl, requestJson, isCacheEnabled, cancellationToken);
        var completionResponse = new CompletionResponse
        {
            StatusCode = response.StatusCode,
            ResponseBody = response.Body,
            IsCacheHit = isCacheHit
        };

        if (!isCacheHit)
        {
            var endedAt = DateTime.UtcNow;
            completionResponse.Log = new CompletionOutgoingLog
            {
                Id = RoutifyId.Generate(IdType.CompletionOutgoingLog),
                IncomingLogId = request.LogId,
                AppId = request.Context.App.Id,
                RouteId = request.Context.Route.Id,
                Provider = request.AppProvider.Provider,
                AppProviderId = request.AppProvider.Id,
                RouteProviderId = request.RouteProvider.Id,
                RequestUrl = requestUrl,
                RequestMethod = "POST",
                RequestBody = requestJson,
                StatusCode = completionResponse.StatusCode,
                ResponseBody = completionResponse.ResponseBody,
                ResponseHeaders = completionResponse.ResponseHeaders,
                StartedAt = startedAt,
                EndedAt = endedAt,
                Duration = (endedAt - startedAt).TotalMilliseconds
            };
        }
        
        if (!HttpUtils.IsSuccessStatusCode(response.StatusCode))
            return completionResponse;
        
        var responseOutput = RoutifyJsonSerializer.Deserialize<TOutput>(response.Body);
        if (responseOutput == null)
            return completionResponse;

        completionResponse.Output = responseOutput;
        completionResponse.Model = GetModel(input, responseOutput);
        completionResponse.InputTokens = GetInputTokens(responseOutput);
        completionResponse.OutputTokens = GetOutputTokens(responseOutput);

        if (!Models.TryGetValue(completionResponse.Model, out var model)) 
            return completionResponse;
        
        completionResponse.InputCost = model.InputCost / model.InputCostUnit * completionResponse.InputTokens;
        completionResponse.OutputCost = model.OutputCost / model.OutputCostUnit * completionResponse.OutputTokens;

        return completionResponse;
    }
    
    private async Task<(HttpCompletionResponse, bool)> GetResponseAsync(
        CompletionRequest request,
        string requestUrl,
        string requestJson,
        bool isCacheEnabled,
        CancellationToken cancellationToken)
    {
        if (isCacheEnabled)
        {
            var hash = $"{requestUrl}_{requestJson}".ToSha256();
            var key = $"{request.Context.Route.Id}_{hash}";
            var cacheResponse = await request.Context.Cache.GetAsync<HttpCompletionResponse>(key);
            if (cacheResponse != null)
                return (cacheResponse, true);
        }
        
        var httpClient = PrepareHttpClient(request);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync(requestUrl, requestContent, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var completionResponse = new HttpCompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            Body = responseBody,
        };
        
        if (isCacheEnabled)
        {
            var hash = $"{requestUrl}_{requestJson}".ToSha256();
            var key = $"{request.Context.Route.Id}_{hash}";
            var expirationSeconds = request.Context.Route.CacheConfig?.Expiration ?? 60;
            var expiration = TimeSpan.FromSeconds(expirationSeconds);
            await request.Context.Cache.SetAsync(key, completionResponse, expiration);
        }
        
        return (completionResponse, false);
    }

    public abstract ICompletionInput? ParseInput(string input);
    public abstract string SerializeOutput(ICompletionOutput output);

    protected abstract HttpClient PrepareHttpClient(CompletionRequest request);
    protected abstract string PrepareRequestUrl(CompletionRequest request);
    protected abstract TInput PrepareInput(CompletionRequest request);
    protected abstract int GetInputTokens(TOutput output);
    protected abstract int GetOutputTokens(TOutput output);
    protected abstract string GetModel(TInput input, TOutput output);
    
    private record HttpCompletionResponse
    {
        public int StatusCode { get; init; }
        public string Body { get; init; } = null!;
    }
}