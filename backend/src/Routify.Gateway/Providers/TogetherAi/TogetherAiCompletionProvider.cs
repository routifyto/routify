using System.Net;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionProvider(
    IHttpClientFactory httpClientFactory,
    [FromKeyedServices(ProviderIds.TogetherAi)] ICompletionInputMapper inputMapper) 
    : ICompletionProvider
{
    private static readonly Dictionary<string, decimal> ModelInputCosts = new()
    {
        //TODO
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
        //TODO
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

        var client = httpClientFactory.CreateClient(ProviderIds.TogetherAi);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var togetherAiInput = inputMapper.Map(request.Input) as TogetherAiCompletionInput;
        if (togetherAiInput == null)
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            };
        }
        
        if (!string.IsNullOrWhiteSpace(request.Model))
            togetherAiInput.Model = request.Model;

        if (!request.RouteProviderAttrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            togetherAiInput.Messages.Insert(0, new TogetherAiCompletionMessageInput
            {
                Content = systemPrompt,
                Role = "system"
            });
        }

        if (request.RouteProviderAttrs.TryGetValue("temperature", out var temperatureString) 
            && !string.IsNullOrWhiteSpace(temperatureString) 
            && float.TryParse(temperatureString, out var temperature))
        {
            togetherAiInput.Temperature = temperature;
        }

        if (request.RouteProviderAttrs.TryGetValue("maxTokens", out var maxTokensString) 
            && !string.IsNullOrWhiteSpace(maxTokensString) 
            && int.TryParse(maxTokensString, out var maxTokens))
        {
            togetherAiInput.MaxTokens = maxTokens;
        }

        if (request.RouteProviderAttrs.TryGetValue("frequencyPenalty", out var frequencyPenaltyString) 
            && !string.IsNullOrWhiteSpace(frequencyPenaltyString) 
            && float.TryParse(frequencyPenaltyString, out var frequencyPenalty))
        {
            togetherAiInput.FrequencyPenalty = frequencyPenalty;
        }

        if (request.RouteProviderAttrs.TryGetValue("presencePenalty", out var presencePenaltyString) 
            && !string.IsNullOrWhiteSpace(presencePenaltyString) 
            && float.TryParse(presencePenaltyString, out var presencePenalty))
        {
            togetherAiInput.PresencePenalty = presencePenalty;
        }
        
        var response = await client.PostAsJsonAsync("chat/completions", togetherAiInput, cancellationToken);
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