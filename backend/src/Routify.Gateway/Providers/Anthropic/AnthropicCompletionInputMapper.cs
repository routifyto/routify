using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Anthropic;

internal class AnthropicCompletionInputMapper : ICompletionInputMapper
{
    public ICompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            AnthropicCompletionInput _ => input,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static AnthropicCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        var systemMessages = input
            .Messages
            .Where(message => message.Role == "system")
            .ToList();
        
        var systemPrompt = string.Join("\n", systemMessages.Select(message => message.Content));
        
        var otherMessages = input
            .Messages
            .Where(message => message.Role != "system")
            .ToList();
        
        return new AnthropicCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = otherMessages
                .Select(message => new AnthropicCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList(),
            System = systemPrompt,
        };
    }

    private static AnthropicCompletionInput MapTogetherAiCompletionInput(
        TogetherAiCompletionInput input)
    {
        var systemMessages = input
            .Messages
            .Where(message => message.Role == "system")
            .ToList();
        
        var systemPrompt = string.Join("\n", systemMessages.Select(message => message.Content));
        
        var otherMessages = input
            .Messages
            .Where(message => message.Role != "system")
            .ToList();
        
        return new AnthropicCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = otherMessages
                .Select(message => new AnthropicCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList(),
            System = systemPrompt,
        };
    }
}