using System.Net;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Cohere.Models;

namespace Routify.Gateway.Providers.Cohere;

internal class CohereCompletionProvider(
    IHttpClientFactory httpClientFactory,
    [FromKeyedServices(ProviderIds.OpenAi)] ICompletionInputMapper inputMapper) 
    : ICompletionProvider
{
    private static readonly Dictionary<string, decimal> ModelInputCosts = new()
    {
        { "command-r-plus", 3m },
        { "command-r", 0.5m },
        { "command", 0m },
        { "command-nightly", 0m },
        { "command-light", 0m },
        { "command-light-nightly", 0m },
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
        { "command-r-plus", 15m },
        { "command-r", 1.5m },
        { "command", 0m },
        { "command-nightly", 0m },
        { "command-light", 0m },
        { "command-light-nightly", 0m },
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

        var cohereAiInput = inputMapper.Map(request.Input) as CohereCompletionInput;
        if (cohereAiInput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        
        if (!string.IsNullOrWhiteSpace(request.Model))
            cohereAiInput.Model = request.Model;

        if (!request.RouteProviderAttrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            cohereAiInput.ChatHistory.Insert(0, new CohereCompletionMessageInput
            {
                Message = systemPrompt,
                Role = "SYSTEM"
            });
        }

        if (request.RouteProviderAttrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            cohereAiInput.Temperature = temperature;
        }

        if (request.RouteProviderAttrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            cohereAiInput.MaxTokens = maxTokens;
        }

        if (request.RouteProviderAttrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString) 
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString) 
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            cohereAiInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProviderAttrs.TryGetValue("presencePenalty", out var presencePenaltyString) 
            && !string.IsNullOrWhiteSpace(presencePenaltyString) 
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            cohereAiInput.PresencePenalty = presencePenalty;
        }
        
        var response = await client.PostAsJsonAsync("chat", cohereAiInput, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return new CompletionResponse
            {
                StatusCode = (int)response.StatusCode,
                Error = responseBody,
            };
        }

        var responseOutput = JsonSerializer.Deserialize<CohereCompletionOutput>(responseBody);
        if (responseOutput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
            };
        }

        var usage = responseOutput.Meta?.BilledUnits;
        var model = cohereAiInput.Model;
        var completionResponse = new CompletionResponse
        {
            StatusCode = (int)response.StatusCode,
            Model = cohereAiInput.Model,
            Output = responseOutput
        };

        if (string.IsNullOrWhiteSpace(model) || usage == null) 
            return completionResponse;
        
        completionResponse.InputTokens = usage.InputTokens;
        completionResponse.OutputTokens = usage.OutputTokens;
        completionResponse.InputCost = CalculateInputCost(model, usage.InputTokens);
        completionResponse.OutputCost = CalculateOutputCost(model, usage.OutputTokens);

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