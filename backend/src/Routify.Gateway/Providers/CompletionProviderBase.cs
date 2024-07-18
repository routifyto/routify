using System.Text;
using Routify.Core.Utils;
using Routify.Gateway.Abstractions;

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
        var httpClient = PrepareHttpClient(request);
        var input = PrepareInput(request);
        
        var requestUrl = PrepareRequestUrl(request);
        var requestJson = RoutifyJsonSerializer.Serialize(input);
        var requestContent = new StringContent(requestJson, Encoding.UTF8, "application/json");
        
        var response = await httpClient.PostAsync(requestUrl, requestContent, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        
        var completionResponse = new CompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            RequestUrl = response.RequestMessage?.RequestUri?.ToString(),
            RequestBody = requestJson,
            RequestMethod = response.RequestMessage?.Method?.Method,
            ResponseBody = responseBody,
        };
        
        if (!response.IsSuccessStatusCode)
            return completionResponse;
        
        var responseOutput = RoutifyJsonSerializer.Deserialize<TOutput>(responseBody);
        if (responseOutput == null)
            return completionResponse;

        completionResponse.Output = responseOutput;
        completionResponse.Model = GetModel(input, responseOutput);
        completionResponse.InputTokens = GetInputTokens(responseOutput);
        completionResponse.OutputTokens = GetOutputTokens(responseOutput);
        
        if (Models.TryGetValue(completionResponse.Model, out var model))
        {
            completionResponse.InputCost = model.InputCost / model.InputCostUnit * completionResponse.InputTokens;
            completionResponse.OutputCost = model.OutputCost / model.OutputCostUnit * completionResponse.OutputTokens;
        }
        
        return completionResponse;
    }

    public abstract ICompletionInput? ParseInput(string input);
    public abstract string SerializeOutput(ICompletionOutput output);

    protected abstract HttpClient PrepareHttpClient(CompletionRequest request);
    protected abstract string PrepareRequestUrl(CompletionRequest request);
    protected abstract TInput PrepareInput(CompletionRequest request);
    protected abstract int GetInputTokens(TOutput output);
    protected abstract int GetOutputTokens(TOutput output);
    protected abstract string GetModel(TInput input, TOutput output);
}