using System.Text.Json;
using Routify.Provider.Core;
using Routify.Provider.Core.Models;
using Routify.Provider.OpenAi.Models;

namespace Routify.Provider.OpenAi;

internal class OpenAiCompletionSerializer : ICompletionSerializer
{
    public CompletionInput? Parse(
        string input)
    {
        var openAiInput = JsonSerializer.Deserialize<OpenAiCompletionInput>(input);
        if (openAiInput == null)
            return null;

        return new CompletionInput
        {
            Model = openAiInput.Model,
            Messages = openAiInput
                .Messages
                .Select(message => new CompletionMessageInput
                {
                    Content = message.Content,
                    Name = message.Name,
                    Role = message.Role
                })
                .ToList(),
            TopP = openAiInput.TopP,
            N = openAiInput.N,
            Stop = openAiInput.Stop,
            MaxTokens = openAiInput.MaxTokens,

            PresencePenalty = openAiInput.PresencePenalty,
            FrequencyPenalty = openAiInput.FrequencyPenalty,
            Temperature = openAiInput.Temperature
        };
    }
    
    public string Serialize(
        CompletionPayload payload,
        JsonSerializerOptions? options = default)
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
        
        return JsonSerializer.Serialize(openAiPayload, options);
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