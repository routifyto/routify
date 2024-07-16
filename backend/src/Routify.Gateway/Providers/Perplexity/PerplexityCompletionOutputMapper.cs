using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Perplexity;

internal class PerplexityCompletionOutputMapper
{
    public static PerplexityCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            PerplexityCompletionOutput perplexityCompletionOutput => perplexityCompletionOutput,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionOutput(openAiCompletionOutput),
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            AnthropicCompletionOutput anthropicCompletionOutput => MapAnthropicCompletionOutput(anthropicCompletionOutput),
            MistralCompletionOutput mistralAiCompletionOutput => MapMistralAiCompletionOutput(mistralAiCompletionOutput),
            GroqCompletionOutput groqCompletionOutput => MapGroqCompletionOutput(groqCompletionOutput),
            CloudflareCompletionOutput cloudflareCompletionOutput => MapCloudflareCompletionOutput(cloudflareCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }

    private static PerplexityCompletionOutput MapOpenAiCompletionOutput(
        OpenAiCompletionOutput output)
    {
        return new PerplexityCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new PerplexityCompletionChoiceOutput
                {
                    Index = index,
                    Message = new PerplexityCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new PerplexityCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static PerplexityCompletionOutput MapTogetherAiCompletionOutput(
        TogetherAiCompletionOutput output)
    {
        return new PerplexityCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new PerplexityCompletionChoiceOutput
                {
                    Index = index,
                    Message = new PerplexityCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new PerplexityCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static PerplexityCompletionOutput MapAnthropicCompletionOutput(
        AnthropicCompletionOutput output)
    {
        var textContents = output
            .Content
            .Where(x => x.Type == "text" && !string.IsNullOrWhiteSpace(x.Text))
            .ToList();
        
        var text = string.Join(" ", textContents.Select(x => x.Text));
        
        return new PerplexityCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Type,
            Created = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds(),
            Choices = [
                new PerplexityCompletionChoiceOutput
                {
                    Index = 0,
                    Message = new PerplexityCompletionMessageOutput
                    {
                        Role = output.Role,
                        Content = text
                    },
                    FinishReason = output.StopReason,
                }
            ],
            Usage = new PerplexityCompletionUsageOutput
            {
                CompletionTokens = output.Usage.OutputTokens,
                PromptTokens = output.Usage.InputTokens,
                TotalTokens = output.Usage.InputTokens + output.Usage.OutputTokens
            }
        };
    }
    
    private static PerplexityCompletionOutput MapMistralAiCompletionOutput(
        MistralCompletionOutput output)
    {
        return new PerplexityCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new PerplexityCompletionChoiceOutput
                {
                    Index = index,
                    Message = new PerplexityCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new PerplexityCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static PerplexityCompletionOutput MapGroqCompletionOutput(
        GroqCompletionOutput output)
    {
        return new PerplexityCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new PerplexityCompletionChoiceOutput
                {
                    Index = index,
                    Message = new PerplexityCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new PerplexityCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static PerplexityCompletionOutput MapCloudflareCompletionOutput(
        CloudflareCompletionOutput output)
    {
        return new PerplexityCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new PerplexityCompletionChoiceOutput
                {
                    Index = index,
                    Message = new PerplexityCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content,
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new PerplexityCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
}