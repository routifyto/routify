using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.MistralAi.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.MistralAi;

internal class MistralAiCompletionOutputMapper
{
    public static MistralAiCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            MistralAiCompletionOutput mistralAiCompletionOutput => mistralAiCompletionOutput,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionOutput(openAiCompletionOutput),
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            AnthropicCompletionOutput anthropicCompletionOutput => MapAnthropicCompletionOutput(anthropicCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }

    private static MistralAiCompletionOutput MapOpenAiCompletionOutput(
        OpenAiCompletionOutput output)
    {
        return new MistralAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new MistralAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new MistralAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new MistralAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static MistralAiCompletionOutput MapTogetherAiCompletionOutput(
        TogetherAiCompletionOutput output)
    {
        return new MistralAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Object,
            Created = output.Created,
            Choices = output
                .Choices
                .Select((choice, index) => new MistralAiCompletionChoiceOutput
                {
                    Index = index,
                    Message = new MistralAiCompletionMessageOutput
                    {
                        Role = choice.Message.Role,
                        Content = choice.Message.Content
                    },
                    FinishReason = choice.FinishReason,
                })
                .ToList(),
            Usage = new MistralAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.CompletionTokens,
                PromptTokens = output.Usage.PromptTokens,
                TotalTokens = output.Usage.TotalTokens
            }
        };
    }
    
    private static MistralAiCompletionOutput MapAnthropicCompletionOutput(
        AnthropicCompletionOutput output)
    {
        var textContents = output
            .Content
            .Where(x => x.Type == "text" && !string.IsNullOrWhiteSpace(x.Text))
            .ToList();
        
        var text = string.Join(" ", textContents.Select(x => x.Text));
        
        return new MistralAiCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Object = output.Type,
            Created = TimeProvider.System.GetUtcNow().ToUnixTimeSeconds(),
            Choices = [
                new MistralAiCompletionChoiceOutput
                {
                    Index = 0,
                    Message = new MistralAiCompletionMessageOutput
                    {
                        Role = output.Role,
                        Content = text
                    },
                    FinishReason = output.StopReason,
                }
            ],
            Usage = new MistralAiCompletionUsageOutput
            {
                CompletionTokens = output.Usage.OutputTokens,
                PromptTokens = output.Usage.InputTokens,
                TotalTokens = output.Usage.InputTokens + output.Usage.OutputTokens
            }
        };
    }
}