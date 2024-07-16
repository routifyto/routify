using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Anthropic;

internal class AnthropicCompletionOutputMapper
{
    public static AnthropicCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            AnthropicCompletionOutput anthropicCompletionOutput => anthropicCompletionOutput,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionOutput(openAiCompletionOutput),
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            MistralCompletionOutput mistralAiCompletionOutput => MapMistralAiCompletionOutput(mistralAiCompletionOutput),
            GroqCompletionOutput groqCompletionOutput => MapGroqCompletionOutput(groqCompletionOutput),
            CloudflareCompletionOutput cloudflareCompletionOutput => MapCloudflareCompletionOutput(cloudflareCompletionOutput),
            PerplexityCompletionOutput perplexityCompletionOutput => MapPerplexityCompletionOutput(perplexityCompletionOutput),
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
        MistralCompletionOutput output)
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
    
    private static AnthropicCompletionOutput MapGroqCompletionOutput(
        GroqCompletionOutput output)
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
    
    private static AnthropicCompletionOutput MapCloudflareCompletionOutput(
        CloudflareCompletionOutput output)
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
    
    private static AnthropicCompletionOutput MapPerplexityCompletionOutput(
        PerplexityCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new AnthropicCompletionOutput
        {
            Id = output.Id,
            Model = output.Model,
            Type = output.Object,
            Role = choice?.Message?.Role ?? "assistant",
            Content = [
                new AnthropicCompletionContentOutput
                {
                    Type = "text",
                    Text = choice?.Message?.Content
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