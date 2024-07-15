using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.MistralAi.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Groq;

internal class GroqCompletionInputMapper
{
    public static GroqCompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            GroqCompletionInput groqCompletionInput => groqCompletionInput,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralAiCompletionInput mistralAiCompletionInput => MapMistralAiCompletionInput(mistralAiCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static GroqCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        return new GroqCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Seed = input.Seed,
            User = input.User,
            Messages = input
                .Messages
                .Select(message => new GroqCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static GroqCompletionInput MapTogetherAiCompletionInput(
        TogetherAiCompletionInput input)
    {
        return new GroqCompletionInput
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
                .Select(message => new GroqCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static GroqCompletionInput MapAnthropicCompletionInput(
        AnthropicCompletionInput input)
    {
        var messages = input
            .Messages
            .Select(message => new GroqCompletionMessageInput
            {
                Content = message.Content,
                Role = message.Role
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            messages.Insert(0, new GroqCompletionMessageInput
            {
                Content = input.System,
                Role = "system"
            });
        }
        
        return new GroqCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = messages
        };
    }
    
    private static GroqCompletionInput MapMistralAiCompletionInput(
        MistralAiCompletionInput input)
    {
        return new GroqCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new GroqCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
}