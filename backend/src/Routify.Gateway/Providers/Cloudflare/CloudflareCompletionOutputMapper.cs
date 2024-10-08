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

internal class CloudflareCompletionOutputMapper
{
    public static CloudflareCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            CloudflareCompletionOutput cloudflareCompletionOutput => cloudflareCompletionOutput,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionOutput(openAiCompletionOutput),
            AzureOpenAiCompletionOutput azureOpenAiCompletionOutput => MapAzureOpenAiCompletionOutput(azureOpenAiCompletionOutput),
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            AnthropicCompletionOutput anthropicCompletionOutput => MapAnthropicCompletionOutput(anthropicCompletionOutput),
            MistralCompletionOutput mistralAiCompletionOutput => MapMistralAiCompletionOutput(mistralAiCompletionOutput),
            GroqCompletionOutput groqCompletionOutput => MapGroqCompletionOutput(groqCompletionOutput),
            PerplexityCompletionOutput perplexityCompletionOutput => MapPerplexityCompletionOutput(perplexityCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }

    private static CloudflareCompletionOutput MapOpenAiCompletionOutput(
        OpenAiCompletionOutput output)
    {
        return new CloudflareCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new CloudflareCompletionChoiceOutput
                {
                    Index = index,
                    Message = new CloudflareCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new CloudflareCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            },
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint
        };
    }
    
    private static CloudflareCompletionOutput MapAzureOpenAiCompletionOutput(
        AzureOpenAiCompletionOutput output)
    {
        return new CloudflareCompletionOutput
        {
            Id = output.Id,
            Model = output.Model ?? string.Empty,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new CloudflareCompletionChoiceOutput
                {
                    Index = index,
                    Message = new CloudflareCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new CloudflareCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            },
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint
        };
    }
    
    private static CloudflareCompletionOutput MapTogetherAiCompletionOutput(
        TogetherAiCompletionOutput output)
    {
        return new CloudflareCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new CloudflareCompletionChoiceOutput
                {
                    Index = index,
                    Message = new CloudflareCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new CloudflareCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static CloudflareCompletionOutput MapAnthropicCompletionOutput(
        AnthropicCompletionOutput output)
    {
        var textContents = output
            .Content
            .Where(x => x.Type == "text" && !string.IsNullOrWhiteSpace(x.Text))
            .ToList();
        
        var text = string.Join(" ", textContents.Select(x => x.Text));
        
        return new CloudflareCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Type,
            Created = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds(),
            Choices = [
                new CloudflareCompletionChoiceOutput
                {
                    Index = 0,
                    Message = new CloudflareCompletionMessageOutput
                    {
                        Role = output.Role,
                        Content = text
                    },
                    FinishReason = output.StopReason,
                }
            ],
            Usage = new CloudflareCompletionUsageOutput
            {
                CompletionTokens = output.Usage.OutputTokens,
                PromptTokens = output.Usage.InputTokens,
                TotalTokens = output.Usage.InputTokens + output.Usage.OutputTokens
            }
        };
    }
    
    private static CloudflareCompletionOutput MapMistralAiCompletionOutput(
        MistralCompletionOutput output)
    {
        return new CloudflareCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new CloudflareCompletionChoiceOutput
                {
                    Index = index,
                    Message = new CloudflareCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new CloudflareCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static CloudflareCompletionOutput MapGroqCompletionOutput(
        GroqCompletionOutput output)
    {
        return new CloudflareCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new CloudflareCompletionChoiceOutput
                {
                    Index = index,
                    Message = new CloudflareCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new CloudflareCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static CloudflareCompletionOutput MapPerplexityCompletionOutput(
        PerplexityCompletionOutput output)
    {
        return new CloudflareCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new CloudflareCompletionChoiceOutput
                {
                    Index = index,
                    Message = new CloudflareCompletionMessageOutput
                    {
                        Role = choice.Message?.Role ?? string.Empty,
                        Content = choice.Message?.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new CloudflareCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            },
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint
        };
    }
}