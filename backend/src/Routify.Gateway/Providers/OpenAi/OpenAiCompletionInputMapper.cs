using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.AzureOpenAi.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
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
            AzureOpenAiCompletionInput azureOpenAiCompletionInput => MapAzureOpenAiCompletionInput(azureOpenAiCompletionInput),
            TogetherAiCompletionInput togetherAiCompletionInput => MapTogetherAiCompletionInput(togetherAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralCompletionInput mistralAiCompletionInput => MapMistralAiCompletionInput(mistralAiCompletionInput),
            GroqCompletionInput groqCompletionInput => MapGroqCompletionInput(groqCompletionInput),
            CloudflareCompletionInput cloudflareCompletionInput => MapCloudflareCompletionInput(cloudflareCompletionInput),
            PerplexityCompletionInput perplexityCompletionInput => MapPerplexityCompletionInput(perplexityCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static OpenAiCompletionInput MapAzureOpenAiCompletionInput(
        AzureOpenAiCompletionInput input)
    {
        return new OpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop != null
                ? new OpenAiCompletionStopInput
                {
                    StringValue = input.Stop.StringValue,
                    ListValue = input.Stop.ListValue
                }
                : null,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Logprobs = input.Logprobs,
            TopLogprobs = input.TopLogprobs,
            Seed = input.Seed,
            User = input.User,
            Messages = input
                .Messages
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = new OpenAiCompletionMessageContentInput
                    {
                        StringValue = message.Content
                    },
                    Role = message.Role
                })
                .ToList()
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
            Stop = input.Stop != null 
                ? new OpenAiCompletionStopInput
                {
                    ListValue = input.Stop
                } 
                : null,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = new OpenAiCompletionMessageContentInput
                    {
                        StringValue = message.Content
                    },
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
            .Where(message => !string.IsNullOrWhiteSpace(message.Content.StringValue))
            .Select(message => new OpenAiCompletionMessageInput
            {
                Content = new OpenAiCompletionMessageContentInput
                {
                    StringValue = message.Content.StringValue
                },
                Role = message.Role
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(input.System))
        {
            messages.Insert(0, new OpenAiCompletionMessageInput
            {
                Content = new OpenAiCompletionMessageContentInput
                {
                    StringValue = input.System
                },
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
        MistralCompletionInput input)
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
                    Content = new OpenAiCompletionMessageContentInput
                    {
                        StringValue = message.Content
                    },
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static OpenAiCompletionInput MapGroqCompletionInput(
        GroqCompletionInput input)
    {
        return new OpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = new OpenAiCompletionStopInput
            {
                StringValue = input.Stop
            },
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Seed = input.Seed,
            User = input.User,
            Messages = input
                .Messages
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = new OpenAiCompletionMessageContentInput
                    {
                        StringValue = message.Content
                    },
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static OpenAiCompletionInput MapCloudflareCompletionInput(
        CloudflareCompletionInput input)
    {
        return new OpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = new OpenAiCompletionStopInput
            {
                StringValue = input.Stop
            },
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
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = new OpenAiCompletionMessageContentInput
                    {
                        StringValue = message.Content
                    },
                    Role = message.Role
                })
                .ToList()
        };
    }
    
    private static OpenAiCompletionInput MapPerplexityCompletionInput(
        PerplexityCompletionInput input)
    {
        return new OpenAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Select(message => new OpenAiCompletionMessageInput
                {
                    Content = new OpenAiCompletionMessageContentInput
                    {
                        StringValue = message.Content
                    },
                    Role = message.Role
                })
                .ToList()
        };
    }
}