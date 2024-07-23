using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.AzureOpenAi.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.AzureOpenAi;

internal class AzureOpenAiCompletionInputMapper
{
    public static AzureOpenAiCompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            AzureOpenAiCompletionInput azureOpenAiCompletionInput => azureOpenAiCompletionInput,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralCompletionInput mistralAiCompletionInput => MapMistralAiCompletionInput(mistralAiCompletionInput),
            GroqCompletionInput groqCompletionInput => MapGroqCompletionInput(groqCompletionInput),
            CloudflareCompletionInput cloudflareCompletionInput => MapCloudflareCompletionInput(cloudflareCompletionInput),
            PerplexityCompletionInput perplexityCompletionInput => MapPerplexityCompletionInput(perplexityCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }
    
    private static AzureOpenAiCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        return new AzureOpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop != null 
                ? new AzureOpenAiCompletionStopInput
                {
                    StringValue = input.Stop.StringValue,
                    ListValue = input.Stop.ListValue
                } 
                : null,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Seed = input.Seed,
            User = input.User,
            Logprobs = input.Logprobs,
            TopLogprobs = input.TopLogprobs,
            Messages = input
                .Messages
                .Where(message => !string.IsNullOrWhiteSpace(message.Content.StringValue))
                .Select(message => new AzureOpenAiCompletionMessageInput
                {
                    Content = message.Content.StringValue!,
                    Role = message.Role
                })
                .ToList()
        };
    }

    private static AzureOpenAiCompletionInput MapTogetherAiCompletionInput(
        TogetherAiCompletionInput input)
    {
        return new AzureOpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop != null 
                ? new AzureOpenAiCompletionStopInput
                {
                    ListValue = input.Stop,
                } 
                : null,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new AzureOpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static AzureOpenAiCompletionInput MapAnthropicCompletionInput(
        AnthropicCompletionInput input)
    {
        var messages = input
            .Messages
            .Where(message => !string.IsNullOrWhiteSpace(message.Content.StringValue))
            .Select(message => new AzureOpenAiCompletionMessageInput
            {
                Content = message.Content.StringValue!,
                Role = message.Role
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            messages.Insert(0, new AzureOpenAiCompletionMessageInput
            {
                Content = input.System,
                Role = "system"
            });
        }
        
        return new AzureOpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = messages
        };
    }
    
    private static AzureOpenAiCompletionInput MapMistralAiCompletionInput(
        MistralCompletionInput input)
    {
        return new AzureOpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new AzureOpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static AzureOpenAiCompletionInput MapGroqCompletionInput(
        GroqCompletionInput input)
    {
        return new AzureOpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop != null 
                ? new AzureOpenAiCompletionStopInput
                {
                    StringValue = input.Stop,
                } 
                : null,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Seed = input.Seed,
            User = input.User,
            Messages = input
                .Messages
                .Select(message => new AzureOpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static AzureOpenAiCompletionInput MapCloudflareCompletionInput(
        CloudflareCompletionInput input)
    {
        return new AzureOpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop != null 
                ? new AzureOpenAiCompletionStopInput
                {
                    StringValue = input.Stop,
                } 
                : null,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Seed = input.Seed,
            User = input.User,
            Logprobs = input.Logprobs,
            TopLogprobs = input.TopLogprobs,
            Messages = input
                .Messages
                .Select(message => new AzureOpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static AzureOpenAiCompletionInput MapPerplexityCompletionInput(
        PerplexityCompletionInput input)
    {
        return new AzureOpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new AzureOpenAiCompletionMessageInput
                {
                    Content = message.Content,
                    Role = message.Role
                })
                .ToList()
        };
    }
}