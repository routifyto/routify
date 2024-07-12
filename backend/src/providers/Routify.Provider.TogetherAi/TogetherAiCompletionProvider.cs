using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Provider.Core;
using Routify.Provider.Core.Models;
using Routify.Provider.TogetherAi.Models;

namespace Routify.Provider.TogetherAi;

internal class TogetherAiCompletionProvider(
    IHttpClientFactory httpClientFactory) 
    : ICompletionProvider
{
    private static readonly Dictionary<string, decimal> ModelInputCosts = new()
    {
    };

    private static readonly Dictionary<string, decimal> ModelOutputCosts = new()
    {
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

        var openAiInput = PrepareTogetherAiInput(request);
        var response = await client.PostAsJsonAsync("chat/completions", openAiInput, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var responsePayload = JsonSerializer.Deserialize<TogetherAiCompletionPayload>(responseBody);

        var completionResponse = new CompletionResponse
        {
            Payload = MapToCompletionPayload(responsePayload),
            StatusCode = (int)response.StatusCode,
        };

        if (responsePayload != null)
        {
            var usage = responsePayload.Usage;
            completionResponse.InputTokens = usage.PromptTokens;
            completionResponse.OutputTokens = usage.CompletionTokens;
            completionResponse.InputCost = CalculateInputCost(responsePayload.Model, usage.PromptTokens);
            completionResponse.OutputCost = CalculateOutputCost(responsePayload.Model, usage.CompletionTokens);
        }

        return completionResponse;
    }
    
    private static TogetherAiCompletionInput PrepareTogetherAiInput(
        CompletionRequest request)
    {
        var input = request.Input;
        var openAiInput = new TogetherAiCompletionInput
        {
            Model = input.Model,
            Messages = input
                .Messages
                .Select(message => new TogetherAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList(),
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature
        };

        if (!string.IsNullOrWhiteSpace(request.Model))
            openAiInput.Model = request.Model;

        if (!request.RouteProviderAttrs.TryGetValue("systemPrompt", out var systemPrompt)
            && !string.IsNullOrWhiteSpace(systemPrompt))
        {
            openAiInput.Messages.Insert(0, new TogetherAiCompletionMessageInput
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

        return openAiInput;
    }

    private static CompletionPayload? MapToCompletionPayload(
        TogetherAiCompletionPayload? payload)
    {
        if (payload == null)
            return null;

        return new CompletionPayload
        {
            Id = payload.Id,
            Object = payload.Object,
            Choices = payload
                .Choices
                .Select(choice => new CompletionChoicePayload
                {
                    FinishReason = choice.FinishReason,
                    Message = new CompletionMessagePayload
                    {
                        Content = choice.Message.Content,
                        Role = choice.Message.Role
                    },
                    Logprobs = new CompletionLogprobsPayload()
                })
                .ToList(),
            Created = payload.Created,
            Model = payload.Model,
            Usage = new CompletionUsagePayload
            {
                PromptTokens = payload.Usage.PromptTokens,
                CompletionTokens = payload.Usage.CompletionTokens,
                TotalTokens = payload.Usage.TotalTokens
            }
        };
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