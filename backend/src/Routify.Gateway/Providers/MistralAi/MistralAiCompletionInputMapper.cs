using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.MistralAi.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.MistralAi;

internal class MistralAiCompletionInputMapper
{
    public static MistralAiCompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            MistralAiCompletionInput mistralAiCompletionInput => mistralAiCompletionInput,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static MistralAiCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        return new MistralAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new MistralAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static MistralAiCompletionInput MapTogetherAiCompletionInput(
        TogetherAiCompletionInput input)
    {
        return new MistralAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new MistralAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static MistralAiCompletionInput MapAnthropicCompletionInput(
        AnthropicCompletionInput input)
    {
        var messages = input
            .Messages
            .Select(message => new MistralAiCompletionMessageInput
            {
                Content = message.Content,
                Role = message.Role
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            messages.Insert(0, new MistralAiCompletionMessageInput
            {
                Content = input.System,
                Role = "system"
            });
        }
        
        return new MistralAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = messages
        };
    }
}