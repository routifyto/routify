using System.Text.Json;
using Routify.Provider.Core.Completion;
using Routify.Provider.OpenAi.Models;

namespace Routify.Provider.OpenAi;

internal class OpenAiCompletionPayloadSerializer : ICompletionPayloadSerializer
{
    public string Serialize(
        CompletionPayload payload)
    {
        var openAiPayload = new OpenAiCompletionPayload
        {
            Id = payload.Id,
            Object = payload.Object,
            Choices = payload
                .Choices
                .Select(choice => new OpenAiCompletionChoicePayload
                {
                    Index = choice.Index,
                    FinishReason = choice.FinishReason,
                    Message = new OpenAiCompletionMessagePayload
                    {
                        Content = choice.Message.Content,
                        Role = choice.Message.Role
                    },
                    Logprobs = new OpenAiCompletionLogpropsPayload
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
            Usage = new OpenAiCompletionUsagePayload
            {
                PromptTokens = payload.Usage.PromptTokens,
                CompletionTokens = payload.Usage.CompletionTokens,
                TotalTokens = payload.Usage.TotalTokens
            }
        };
        
        return JsonSerializer.Serialize(openAiPayload);
    }
    
    private static OpenAiCompletionLogprobsContentPayload MapLogprobsContent(
        CompletionLogprobsContentPayload content)
    {
        return new OpenAiCompletionLogprobsContentPayload
        {
            Token = content.Token,
            Logprob = content.Logprob,
            Bytes = content.Bytes,
            TopLogprobs = content.TopLogprobs.Select(MapLogprobsContent).ToList()
        };
    }
}