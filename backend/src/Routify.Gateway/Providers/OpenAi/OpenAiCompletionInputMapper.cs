using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.MistralAi.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.OpenAi;

internal class OpenAiCompletionInputMapper
{
    public static OpenAiCompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            OpenAiCompletionInput openAiCompletionInput => openAiCompletionInput,
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralAiCompletionInput mistralAiCompletionInput => MapMistralAiCompletionInput(mistralAiCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static OpenAiCompletionInput MapTogetherAiCompletionInput(
        TogetherAiCompletionInput input)
    {
        return new OpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static OpenAiCompletionInput MapAnthropicCompletionInput(
        AnthropicCompletionInput input)
    {
        var messages = input
            .Messages
            .Select(message => new OpenAiCompletionMessageInput
            {
                Content = message.Content,
                Role = message.Role
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            messages.Insert(0, new OpenAiCompletionMessageInput
            {
                Content = input.System,
                Role = "system"
            });
        }
        
        return new OpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = messages
        };
    }
    
    private static OpenAiCompletionInput MapMistralAiCompletionInput(
        MistralAiCompletionInput input)
    {
        return new OpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
}