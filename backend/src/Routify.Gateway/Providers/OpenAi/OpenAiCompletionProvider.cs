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
        { "gpt-4o", 0.000005m },
        { "gpt-4o-2024-05-13", 0.000005m },

        { "gpt-4-turbo", 0.00001m },
        { "gpt-4-turbo-2024-04-09", 0.00001m },
        { "gpt-4-turbo-preview", 0.00001m },
        { "gpt-4-0125-preview", 0.00001m },
        { "gpt-4", 0.00003m },
        { "gpt-4-0613", 0.00003m },
        { "gpt-4-0314", 0.00003m },

        { "gpt-3.5-turbo-0125", 0.0000005m },
        { "gpt-3.5-turbo", 0.0000005m },
        { "gpt-3.5-turbo-1106", 0.000001m },
        { "gpt-3.5-turbo-instruct", 0.0000015m },
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
        { "gpt-4o", 0.000015m },
        { "gpt-4o-2024-05-13", 0.000015m },

        { "gpt-4-turbo", 0.00003m },
        { "gpt-4-turbo-2024-04-09", 0.00003m },
        { "gpt-4-turbo-preview", 0.00003m },
        { "gpt-4-0125-preview", 0.00003m },
        { "gpt-4", 0.00006m },
        { "gpt-4-0613", 0.00006m },
        { "gpt-4-0314", 0.00006m },

        { "gpt-3.5-turbo-0125", 0.0000015m },
        { "gpt-3.5-turbo", 0.0000015m },
        { "gpt-3.5-turbo-1106", 0.000002m },
        { "gpt-3.5-turbo-instruct", 0.000002m },
    };
    
    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request, 
        CancellationToken cancellationToken)
    {
        if (!request.AppProviderAttrs.TryGetValue("apiKey", out var openAiApiKey))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }

        var client = httpClientFactory.CreateClient(ProviderIds.OpenAi);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");

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
            && double.TryParse(temperatureString, out var temperature))
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
            return cost * tokens;
        }

        return 0;
    }

    private static decimal CalculateOutputCost(
        string model,
        int tokens)
    {
        if (ModelOutputCosts.TryGetValue(model, out var cost))
        {
            return cost * tokens;
        }

        return 0;
    }
}