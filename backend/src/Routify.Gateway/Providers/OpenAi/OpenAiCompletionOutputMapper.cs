using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.MistralAi.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.OpenAi;

internal class OpenAiCompletionOutputMapper
{
    public static OpenAiCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            OpenAiCompletionOutput openAiCompletionOutput => openAiCompletionOutput,
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            AnthropicCompletionOutput anthropicCompletionOutput => MapAnthropicCompletionOutput(anthropicCompletionOutput),
            MistralAiCompletionOutput mistralAiCompletionOutput => MapMistralAiCompletionOutput(mistralAiCompletionOutput),
            GroqCompletionOutput groqCompletionOutput => MapGroqCompletionOutput(groqCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }

    private static OpenAiCompletionOutput MapTogetherAiCompletionOutput(
        TogetherAiCompletionOutput output)
    {
        return new OpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new OpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new OpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new OpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static OpenAiCompletionOutput MapAnthropicCompletionOutput(
        AnthropicCompletionOutput output)
    {
        var textContents = output
            .Content
            .Where(x => x.Type == "text" && !string.IsNullOrWhiteSpace(x.Text))
            .ToList();
        
        var text = string.Join(" ", textContents.Select(x => x.Text));
        
        return new OpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Type,
            Created = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds(),
            Choices = [
                new OpenAiCompletionChoiceOutput
                {
                    Index = 0,
                    Message = new OpenAiCompletionMessageOutput
                    {
                        Role = output.Role,
                        Content = text
                    },
                    FinishReason = output.StopReason,
                }
            ],
            Usage = new OpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.OutputTokens,
                PromptTokens = output.Usage.InputTokens,
                TotalTokens = output.Usage.InputTokens + output.Usage.OutputTokens
            }
        };
    }
    
    private static OpenAiCompletionOutput MapMistralAiCompletionOutput(
        MistralAiCompletionOutput output)
    {
        return new OpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new OpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new OpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new OpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static OpenAiCompletionOutput MapGroqCompletionOutput(
        GroqCompletionOutput output)
    {
        return new OpenAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            ServiceTier = output.ServiceTier,
            SystemFingerprint = output.SystemFingerprint,
            Choices = output
                .Choices
                .Select((choice, index) => new OpenAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new OpenAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new OpenAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
}