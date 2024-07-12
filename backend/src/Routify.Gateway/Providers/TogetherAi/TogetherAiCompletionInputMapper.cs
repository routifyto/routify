using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.MistralAi.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionInputMapper : ICompletionInputMapper
{
    public ICompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            TogetherAiCompletionInput _ => input,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralAiCompletionInput mistralAiCompletionInput => MapMistralAiCompletionInput(mistralAiCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static TogetherAiCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        return new TogetherAiCompletionInput
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
                .Select(message => new TogetherAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static TogetherAiCompletionInput MapAnthropicCompletionInput(
        AnthropicCompletionInput input)
    {
        var messages = input
            .Messages
            .Select(message => new TogetherAiCompletionMessageInput
            {
                Content = message.Content,
                Role = message.Role
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            messages.Insert(0, new TogetherAiCompletionMessageInput
            {
                Content = input.System,
                Role = "system"
            });
        }
        
        return new TogetherAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = messages
        };
    }
    
    private static TogetherAiCompletionInput MapMistralAiCompletionInput(
        MistralAiCompletionInput input)
    {
        return new TogetherAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new TogetherAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
}