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
    private static readonly Dictionary<string, double> ModelInputCosts = new()
    {
        { "gpt-4o", 0.000005 },
        { "gpt-4o-2024-05-13", 0.000005 },
        
        { "gpt-4-turbo", 0.00001 },
        { "gpt-4-turbo-2024-04-09", 0.00001 },
        { "gpt-4-turbo-preview", 0.00001 },
        { "gpt-4-0125-preview", 0.00001 },
        { "gpt-4", 0.00003 },
        { "gpt-4-0613", 0.00003 },
        { "gpt-4-0314", 0.00003 },
        
        { "gpt-3.5-turbo-0125", 0.0000005 },
        { "gpt-3.5-turbo", 0.0000005 },
        { "gpt-3.5-turbo-1106", 0.000001 },
        { "gpt-3.5-turbo-instruct", 0.0000015 },
    };
    
    private static readonly Dictionary<string, double> ModelOutputCosts = new()
    {
        { "gpt-4o", 0.000015 },
        { "gpt-4o-2024-05-13", 0.000015 },
        
        { "gpt-4-turbo", 0.00003 },
        { "gpt-4-turbo-2024-04-09", 0.00003 },
        { "gpt-4-turbo-preview", 0.00003 },
        { "gpt-4-0125-preview", 0.00003 },
        { "gpt-4", 0.00006 },
        { "gpt-4-0613", 0.00006 },
        { "gpt-4-0314", 0.00006 },
        
        { "gpt-3.5-turbo-0125", 0.0000015 },
        { "gpt-3.5-turbo", 0.0000015 },
        { "gpt-3.5-turbo-1106", 0.000002 },
        { "gpt-3.5-turbo-instruct", 0.000002 },
    };
    
    public async Task<CompletionResponse> CompleteAsync(
        CompletionRequest request, 
        CancellationToken cancellationToken)
    {
        if (!request.ProviderAttrs.TryGetValue("apiKey", out var openAiApiKey))
        {
            return new CompletionResponse
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
            };
        }
        
        var client = httpClientFactory.CreateClient(ProviderIds.OpenAi);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {openAiApiKey}");
        
        var openAiInput = MapToOpenAiInput(request.Input);
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
    
    private static OpenAiCompletionInput MapToOpenAiInput(
        CompletionInput input)
    {
        return new OpenAiCompletionInput
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

    private static double CalculateInputCost(
        string model,
        int tokens)
    {
        if (ModelInputCosts.TryGetValue(model, out var cost))
        {
            return cost * tokens;
        }
        
        return 0;
    }
    
    private static double CalculateOutputCost(
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