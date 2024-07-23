using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.AzureOpenAi.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionInputMapper
{
    public static TogetherAiCompletionInput Map(
        ICompletionInput input)
    {
        return input switch
        {
            TogetherAiCompletionInput togetherAiCompletionInput => togetherAiCompletionInput,
            OpenAiCompletionInput openAiCompletionInput => MapOpenAiCompletionInput(openAiCompletionInput),
            AzureOpenAiCompletionInput azureOpenAiCompletionInput => MapAzureOpenAiCompletionInput(
                azureOpenAiCompletionInput),
            AnthropicCompletionInput anthropicCompletionInput => MapAnthropicCompletionInput(anthropicCompletionInput),
            MistralCompletionInput mistralAiCompletionInput => MapMistralAiCompletionInput(mistralAiCompletionInput),
            GroqCompletionInput groqCompletionInput => MapGroqCompletionInput(groqCompletionInput),
            CloudflareCompletionInput cloudflareCompletionInput => MapCloudflareCompletionInput(
                cloudflareCompletionInput),
            PerplexityCompletionInput perplexityCompletionInput => MapPerplexityCompletionInput(
                perplexityCompletionInput),
            _ => throw new NotSupportedException($"Input type {input.GetType().Name} is not supported.")
        };
    }

    private static TogetherAiCompletionInput MapOpenAiCompletionInput(
        OpenAiCompletionInput input)
    {
        List<string>? stop = null;
        if (input.Stop != null)
        {
            if (input.Stop.StringValue != null)
            {
                stop ??= [];
                stop.Add(input.Stop.StringValue);
            }
            
            if (input.Stop.ListValue != null)
            {
                stop ??= [];
                stop.AddRange(input.Stop.ListValue);
            }
        }
        
        return new TogetherAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = stop,
            MaxTokens = input.MaxTokens,
            PresencePenalty = input.PresencePenalty,
            FrequencyPenalty = input.FrequencyPenalty,
            Temperature = input.Temperature,
            Messages = input
                .Messages
                .Where(message => !string.IsNullOrWhiteSpace(message.Content.StringValue))
                .Select(message => new TogetherAiCompletionMessageInput
                {
                    Content = message.Content.StringValue!,
                    Role = message.Role
                })
                .ToList()
        };
    }

    private static TogetherAiCompletionInput MapAzureOpenAiCompletionInput(
        AzureOpenAiCompletionInput input)
    {
        List<string>? stop = null;
        if (input.Stop != null)
        {
            if (input.Stop.StringValue != null)
            {
                stop ??= [];
                stop.Add(input.Stop.StringValue);
            }
            
            if (input.Stop.ListValue != null)
            {
                stop ??= [];
                stop.AddRange(input.Stop.ListValue);
            }
        }
        
        return new TogetherAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = stop,
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
            .Where(message => !string.IsNullOrWhiteSpace(message.Content.StringValue))
            .Select(message => new TogetherAiCompletionMessageInput
            {
                Content = message.Content.StringValue!,
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
        MistralCompletionInput input)
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

    private static TogetherAiCompletionInput MapGroqCompletionInput(
        GroqCompletionInput input)
    {
        List<string>? stop = null;
        if (input.Stop != null)
        {
            if (input.Stop.StringValue != null)
            {
                stop ??= [];
                stop.Add(input.Stop.StringValue);
            }
            
            if (input.Stop.ListValue != null)
            {
                stop ??= [];
                stop.AddRange(input.Stop.ListValue);
            }
        }
        
        return new TogetherAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = stop,
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

    private static TogetherAiCompletionInput MapCloudflareCompletionInput(
        CloudflareCompletionInput input)
    {
        return new TogetherAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
            N = input.N,
            Stop = input.Stop != null
                ? [input.Stop]
                : null,
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

    private static TogetherAiCompletionInput MapPerplexityCompletionInput(
        PerplexityCompletionInput input)
    {
        return new TogetherAiCompletionInput
        {
            Model = input.Model,
            TopP = input.TopP,
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
}