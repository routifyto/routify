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

internal class AzureOpenAiCompletionOutputMapper
{
    public static AzureOpenAiCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            AzureOpenAiCompletionOutput azureOpenAiCompletionOutput => azureOpenAiCompletionOutput,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionInput(openAiCompletionOutput),
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            AnthropicCompletionOutput anthropicCompletionOutput => MapAnthropicCompletionOutput(anthropicCompletionOutput),
            MistralCompletionOutput mistralAiCompletionOutput => MapMistralAiCompletionOutput(mistralAiCompletionOutput),
            GroqCompletionOutput groqCompletionOutput => MapGroqCompletionOutput(groqCompletionOutput),
            CloudflareCompletionOutput cloudflareCompletionOutput => MapCloudflareCompletionOutput(cloudflareCompletionOutput),
            PerplexityCompletionOutput perplexityCompletionOutput => MapPerplexityCompletionOutput(perplexityCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }
    
    private static AzureOpenAiCompletionOutput MapOpenAiCompletionInput(
        OpenAiCompletionOutput output)
    {
        return new AzureOpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new AzureOpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new AzureOpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content,
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new AzureOpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }

    private static AzureOpenAiCompletionOutput MapTogetherAiCompletionOutput(
        TogetherAiCompletionOutput output)
    {
        return new AzureOpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new AzureOpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new AzureOpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new AzureOpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static AzureOpenAiCompletionOutput MapAnthropicCompletionOutput(
        AnthropicCompletionOutput output)
    {
        var textContents = output
            .Content
            .Where(x => x.Type == "text" && !string.IsNullOrWhiteSpace(x.Text))
            .ToList();
        
        var text = string.Join(" ", textContents.Select(x => x.Text));
        
        return new AzureOpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Type,
            Created = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds(),
            Choices = [
                new AzureOpenAiCompletionChoiceOutput
                {
                    Index = 0,
                    Message = new AzureOpenAiCompletionMessageOutput
                    {
                        Role = output.Role,
                        Content = text
                    },
                    FinishReason = output.StopReason,
                }
            ],
            Usage = new AzureOpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.OutputTokens,
                PromptTokens = output.Usage.InputTokens,
                TotalTokens = output.Usage.InputTokens + output.Usage.OutputTokens
            }
        };
    }
    
    private static AzureOpenAiCompletionOutput MapMistralAiCompletionOutput(
        MistralCompletionOutput output)
    {
        return new AzureOpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new AzureOpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new AzureOpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new AzureOpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static AzureOpenAiCompletionOutput MapGroqCompletionOutput(
        GroqCompletionOutput output)
    {
        return new AzureOpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new AzureOpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new AzureOpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new AzureOpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static AzureOpenAiCompletionOutput MapCloudflareCompletionOutput(
        CloudflareCompletionOutput output)
    {
        return new AzureOpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new AzureOpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new AzureOpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content,
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new AzureOpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static AzureOpenAiCompletionOutput MapPerplexityCompletionOutput(
        PerplexityCompletionOutput output)
    {
        return new AzureOpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new AzureOpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new AzureOpenAiCompletionMessageOutput
                    {
                        Role = choice.Message?.Role ?? string.Empty,
                        Content = choice.Message?.Content,
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new AzureOpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
}