using System.Net;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.MistralAi.Models;

namespace Routify.Gateway.Providers.MistralAi;

internal class MistralAiCompletionProvider(
    IHttpClientFactory httpClientFactory,
    [FromKeyedServices(ProviderIds.MistralAi)] ICompletionInputMapper inputMapper) 
    : ICompletionProvider
{
    private static readonly Dictionary<string, decimal> ModelInputCosts = new()
    {
        { "open-mistral-7b", 0.25m },
        { "open-mixtral-8x7b", 0.7m },
        { "open-mixtral-8x22b", 2m },
        { "mistral-small-2402", 1m },
        { "codestral-2405", 1m },
        { "mistral-large-2402", 4m },
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
        { "open-mistral-7b", 1m },
        { "open-mixtral-8x7b", 0.7m },
        { "open-mixtral-8x22b", 6m },
        { "mistral-small-2402", 3m },
        { "codestral-2405", 3m },
        { "mistral-large-2402", 12m },
    };
    
    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request, 
        CancellationToken cancellationToken)
    {
        if (!request.AppProviderAttrs.TryGetValue("apiKey", out var apiKey))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }

        var client = httpClientFactory.CreateClient(ProviderIds.OpenAi);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var mistralAiInput = inputMapper.Map(request.Input) as MistralAiCompletionInput;
        if (mistralAiInput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        
        if (!string.IsNullOrWhiteSpace(request.Model))
            mistralAiInput.Model = request.Model;

        if (!request.RouteProviderAttrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            mistralAiInput.Messages.Insert(0, new MistralAiCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProviderAttrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            mistralAiInput.Temperature = temperature;
        }

        if (request.RouteProviderAttrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            mistralAiInput.MaxTokens = maxTokens;
        }

        var response = await client.PostAsJsonAsync("chat/completions", mistralAiInput, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        
        if (!response.IsSuccessStatusCode)
        {
            return new CompletionResponse
            {
                StatusCode = (int)response.StatusCode,
                Error = responseBody,
            };
        }

        var responseOutput = JsonSerializer.Deserialize<MistralAiCompletionOutput>(responseBody);
        if (responseOutput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        var usage = responseOutput.Usage;
        var completionResponse = new CompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            Model = responseOutput.Model,
            InputTokens = usage.PromptTokens,
            OutputTokens = usage.CompletionTokens,
            Output = responseOutput,
            InputCost = CalculateInputCost(responseOutput.Model, usage.PromptTokens),
            OutputCost = CalculateOutputCost(responseOutput.Model, usage.CompletionTokens)
        };

        return completionResponse;
    }
    
    private static decimal CalculateInputCost(
        string model,
        int tokens)
    {
        if (ModelInputCosts.TryGetValue(model, out var cost))
        {
            return cost / 1000000 * tokens;
        }

        return 0;
    }

    private static decimal CalculateOutputCost(
        string model,
        int tokens)
    {
        if (ModelOutputCosts.TryGetValue(model, out var cost))
        {
            return cost / 1000000 * tokens;
        }

        return 0;
    }
}