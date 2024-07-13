using System.Net;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.OpenAi.Models;

namespace Routify.Gateway.Providers.OpenAi;

internal class OpenAiCompletionProvider(
    IHttpClientFactory httpClientFactory,
    [FromKeyedServices(ProviderIds.OpenAi)] ICompletionInputMapper inputMapper) 
    : ICompletionProvider
{
    private static readonly Dictionary<string, decimal> ModelInputCosts = new()
    {
        { "gpt-4o", 5m },
        { "gpt-4o-2024-05-13", 5m },

        { "gpt-4-turbo", 10m },
        { "gpt-4-turbo-2024-04-09", 10m },
        { "gpt-4-turbo-preview", 10m },
        { "gpt-4-0125-preview", 10m },
        { "gpt-4", 30m },
        { "gpt-4-0613", 30m },
        { "gpt-4-0314", 30m },

        { "gpt-3.5-turbo-0125", 0.5m },
        { "gpt-3.5-turbo", 0.5m },
        { "gpt-3.5-turbo-1106", 1m },
        { "gpt-3.5-turbo-instruct", 1.5m },
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
        { "gpt-4o", 15m },
        { "gpt-4o-2024-05-13", 15m },

        { "gpt-4-turbo", 30m },
        { "gpt-4-turbo-2024-04-09", 30m },
        { "gpt-4-turbo-preview", 30m },
        { "gpt-4-0125-preview", 30m },
        { "gpt-4", 60m },
        { "gpt-4-0613", 60m },
        { "gpt-4-0314", 60m },

        { "gpt-3.5-turbo-0125", 1.5m },
        { "gpt-3.5-turbo", 1.5m },
        { "gpt-3.5-turbo-1106", 2m },
        { "gpt-3.5-turbo-instruct", 2m },
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

        var openAiInput = inputMapper.Map(request.Input) as OpenAiCompletionInput;
        if (openAiInput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        
        if (!string.IsNullOrWhiteSpace(request.Model))
            openAiInput.Model = request.Model;

        if (!request.RouteProviderAttrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            openAiInput.Messages.Insert(0, new OpenAiCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProviderAttrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            openAiInput.Temperature = temperature;
        }

        if (request.RouteProviderAttrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            openAiInput.MaxTokens = maxTokens;
        }

        if (request.RouteProviderAttrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString) 
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString) 
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            openAiInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProviderAttrs.TryGetValue("presencePenalty", out var presencePenaltyString) 
            && !string.IsNullOrWhiteSpace(presencePenaltyString) 
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            openAiInput.PresencePenalty = presencePenalty;
        }
        
        var response = await client.PostAsJsonAsync("chat/completions", openAiInput, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var responseOutput = JsonSerializer.Deserialize<OpenAiCompletionOutput>(responseBody);

        var completionResponse = new CompletionResponse
        {
            Output = responseOutput,
            StatusCode = (int)response.StatusCode,
        };

        if (responseOutput != null)
        {
            completionResponse.Model = responseOutput.Model;
            
            var usage = responseOutput.Usage;
            completionResponse.InputTokens = usage.PromptTokens;
            completionResponse.OutputTokens = usage.CompletionTokens;
            completionResponse.InputCost = CalculateInputCost(responseOutput.Model, usage.PromptTokens);
            completionResponse.OutputCost = CalculateOutputCost(responseOutput.Model, usage.CompletionTokens);
        }

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