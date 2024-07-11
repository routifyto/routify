using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Routify.Core.Constants;
using Routify.Provider.Core.Completion;
using Routify.Provider.OpenAi.Models;

namespace Routify.Provider.OpenAi;

internal class OpenAiCompletionProvider(
    IHttpClientFactory httpClientFactory)
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

        var openAiInput = PrepareOpenAiInput(request);
        var response = await client.PostAsJsonAsync("chat/completions", openAiInput, cancellationToken);
        var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
        var responsePayload = JsonSerializer.Deserialize<OpenAiCompletionPayload>(responseBody);

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

    private static OpenAiCompletionInput PrepareOpenAiInput(
        CompletionRequest request)
    {
        var input = request.Input;
        var openAiInput = new OpenAiCompletionInput
        {
            Model = input.Model,
            Messages = input
                .Messages
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Name = message.Name,
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
        OpenAiCompletionPayload? payload)
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
                    Index = choice.Index,
                    FinishReason = choice.FinishReason,
                    Message = new CompletionMessagePayload
                    {
                        Content = choice.Message.Content,
                        Role = choice.Message.Role
                    },
                    Logprobs = new CompletionLogprobsPayload
                    {
                        Content = choice
                            .Logprobs?
                            .Content?
                            .Select(MapLogprobsContent)
                            .ToList()
                    }
                })
                .ToList(),
            Created = payload.Created,
            Model = payload.Model,
            ServiceTier = payload.ServiceTier,
            SystemFingerprint = payload.SystemFingerprint,
            Usage = new CompletionUsagePayload
            {
                PromptTokens = payload.Usage.PromptTokens,
                CompletionTokens = payload.Usage.CompletionTokens,
                TotalTokens = payload.Usage.TotalTokens
            }
        };
    }

    private static CompletionLogprobsContentPayload MapLogprobsContent(
        OpenAiCompletionLogprobsContentPayload content)
    {
        return new CompletionLogprobsContentPayload
        {
            Token = content.Token,
            Logprob = content.Logprob,
            Bytes = content.Bytes,
            TopLogprobs = content.TopLogprobs.Select(MapLogprobsContent).ToList()
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