using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionOutputMapper
{
    public static TogetherAiCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            TogetherAiCompletionOutput togetherAiCompletionOutput => togetherAiCompletionOutput,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionOutput(openAiCompletionOutput),
            AnthropicCompletionOutput anthropicCompletionOutput => MapAnthropicCompletionOutput(anthropicCompletionOutput),
            MistralCompletionOutput mistralAiCompletionOutput => MapMistralAiCompletionOutput(mistralAiCompletionOutput),
            GroqCompletionOutput groqCompletionOutput => MapGroqCompletionOutput(groqCompletionOutput),
            CloudflareCompletionOutput cloudflareCompletionOutput => MapCloudflareCompletionOutput(cloudflareCompletionOutput),
            PerplexityCompletionOutput perplexityCompletionOutput => MapPerplexityCompletionOutput(perplexityCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }

    private static TogetherAiCompletionOutput MapOpenAiCompletionOutput(
        OpenAiCompletionOutput output)
    {
        return new TogetherAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select(choice => new TogetherAiCompletionChoiceOutput
                {
                    FinishReason = choice.FinishReason,
                    Message = new TogetherAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    }
                })
                .ToList(),
            Usage = new TogetherAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static TogetherAiCompletionOutput MapAnthropicCompletionOutput(
        AnthropicCompletionOutput output)
    {
        var textContents = output
            .Content
            .Where(x => x.Type == "text" && !string.IsNullOrWhiteSpace(x.Text))
            .ToList();
        
        var text = string.Join(" ", textContents.Select(x => x.Text));
        
        return new TogetherAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Type,
            Created = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds(),
            Choices = [
                new TogetherAiCompletionChoiceOutput
                {
                    Message = new TogetherAiCompletionMessageOutput
                    {
                        Role = output.Role,
                        Content = text
                    },
                    FinishReason = output.StopReason,
                }
            ],
            Usage = new TogetherAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.OutputTokens,
                PromptTokens = output.Usage.InputTokens,
                TotalTokens = output.Usage.InputTokens + output.Usage.OutputTokens
            }
        };
    }
    
    private static TogetherAiCompletionOutput MapMistralAiCompletionOutput(
        MistralCompletionOutput output)
    {
        return new TogetherAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select(choice => new TogetherAiCompletionChoiceOutput
                {
                    Message = new TogetherAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new TogetherAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static TogetherAiCompletionOutput MapGroqCompletionOutput(
        GroqCompletionOutput output)
    {
        return new TogetherAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select(choice => new TogetherAiCompletionChoiceOutput
                {
                    FinishReason = choice.FinishReason,
                    Message = new TogetherAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    }
                })
                .ToList(),
            Usage = new TogetherAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static TogetherAiCompletionOutput MapCloudflareCompletionOutput(
        CloudflareCompletionOutput output)
    {
        return new TogetherAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select(choice => new TogetherAiCompletionChoiceOutput
                {
                    FinishReason = choice.FinishReason,
                    Message = new TogetherAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    }
                })
                .ToList(),
            Usage = new TogetherAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static TogetherAiCompletionOutput MapPerplexityCompletionOutput(
        PerplexityCompletionOutput output)
    {
        return new TogetherAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select(choice => new TogetherAiCompletionChoiceOutput
                {
                    FinishReason = choice.FinishReason,
                    Message = new TogetherAiCompletionMessageOutput
                    {
                        Role = choice.Message?.Role ?? string.Empty,
                        Content = choice.Message?.Content
                    }
                })
                .ToList(),
            Usage = new TogetherAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
}