using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.MistralAi.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Anthropic;

internal class AnthropicCompletionOutputMapper : ICompletionOutputMapper
{
    public ICompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            AnthropicCompletionOutput _ => output,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionOutput(openAiCompletionOutput),
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            MistralAiCompletionOutput mistralAiCompletionOutput => MapMistralAiCompletionOutput(mistralAiCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }

    private static AnthropicCompletionOutput MapOpenAiCompletionOutput(
        OpenAiCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new AnthropicCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Type = output.Object,
            Role = choice?.Message.Role ?? "assistant",
            Content = [
                new AnthropicCompletionContentOutput
                {
                    Type = "text",
                    Text = choice?.Message.Content
                }
            ],
            StopReason = choice?.FinishReason,
            Usage = new AnthropicCompletionUsageOutput
            {
                InputTokens = output.Usage.PromptTokens,
                OutputTokens = output.Usage.CompletionTokens
            }
        };
    }
    
    private static AnthropicCompletionOutput MapTogetherAiCompletionOutput(
        TogetherAiCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new AnthropicCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Type = output.Object,
            Role = choice?.Message.Role ?? "assistant",
            Content = [
                new AnthropicCompletionContentOutput
                {
                    Type = "text",
                    Text = choice?.Message.Content
                }
            ],
            StopReason = choice?.FinishReason,
            Usage = new AnthropicCompletionUsageOutput
            {
                InputTokens = output.Usage.PromptTokens,
                OutputTokens = output.Usage.CompletionTokens
            }
        };
    }
    
    private static AnthropicCompletionOutput MapMistralAiCompletionOutput(
        MistralAiCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new AnthropicCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Type = output.Object,
            Role = choice?.Message.Role ?? "assistant",
            Content = [
                new AnthropicCompletionContentOutput
                {
                    Type = "text",
                    Text = choice?.Message.Content
                }
            ],
            StopReason = choice?.FinishReason,
            Usage = new AnthropicCompletionUsageOutput
            {
                InputTokens = output.Usage.PromptTokens,
                OutputTokens = output.Usage.CompletionTokens
            }
        };
    }
}