using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.AzureOpenAi.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Cloudflare;

internal class CloudflareCompletionInputMapper
{
    public static CloudflareCompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            CloudflareCompletionInput cloudflareCompletionInput => cloudflareCompletionInput,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            AzureOpenAiCompletionInput azureOpenAiCompletionInput => MapAzureOpenAiCompletionInput(azureOpenAiCompletionInput),
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralCompletionInput mistralAiCompletionInput => MapMistralAiCompletionInput(mistralAiCompletionInput),
            GroqCompletionInput groqCompletionInput => MapGroqCompletionInput(groqCompletionInput),
            PerplexityCompletionInput perplexityCompletionInput => MapPerplexityCompletionInput(perplexityCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }
    
    private static CloudflareCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        return new CloudflareCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop?.StringValue,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Where(message => !string.IsNullOrWhiteSpace(message.Content.StringValue))
                .Select(message => new CloudflareCompletionMessageInput
                {
                    Name = message.Name,
                    Content = message.Content.StringValue!,
                    Role = message.Role
                })
                .ToList(),
            Seed = input.Seed,
            User = input.User,
            Logprobs = input.Logprobs,
            TopLogprobs = input.TopLogprobs,
        };
    }
    
    private static CloudflareCompletionInput MapAzureOpenAiCompletionInput(
        AzureOpenAiCompletionInput input)
    {
        return new CloudflareCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop?.StringValue,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new CloudflareCompletionMessageInput
                {
                    Name = message.Name,
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList(),
            Seed = input.Seed,
            User = input.User,
            Logprobs = input.Logprobs,
            TopLogprobs = input.TopLogprobs,
        };
    }

    private static CloudflareCompletionInput MapTogetherAiCompletionInput(
        TogetherAiCompletionInput input)
    {
        return new CloudflareCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop?.FirstOrDefault(),
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new CloudflareCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static CloudflareCompletionInput MapAnthropicCompletionInput(
        AnthropicCompletionInput input)
    {
        var messages = input
            .Messages
            .Where(message => !string.IsNullOrWhiteSpace(message.Content.StringValue))
            .Select(message => new CloudflareCompletionMessageInput
            {
                Content = message.Content.StringValue!,
                Role = message.Role
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            messages.Insert(0, new CloudflareCompletionMessageInput
            {
                Content = input.System,
                Role = "system"
            });
        }
        
        return new CloudflareCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = messages
        };
    }
    
    private static CloudflareCompletionInput MapMistralAiCompletionInput(
        MistralCompletionInput input)
    {
        return new CloudflareCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new CloudflareCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static CloudflareCompletionInput MapGroqCompletionInput(
        GroqCompletionInput input)
    {
        return new CloudflareCompletionInput
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
                .Select(message => new CloudflareCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static CloudflareCompletionInput MapPerplexityCompletionInput(
        PerplexityCompletionInput input)
    {
        return new CloudflareCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new CloudflareCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList(),
        };
    }
}