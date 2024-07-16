using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;
using Routify.Gateway.Providers.Cloudflare.Models;
using Routify.Gateway.Providers.Cohere.Models;
using Routify.Gateway.Providers.Groq.Models;
using Routify.Gateway.Providers.Mistral.Models;
using Routify.Gateway.Providers.OpenAi.Models;
using Routify.Gateway.Providers.Perplexity.Models;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.Cohere;

internal class CohereCompletionOutputMapper
{
    public static CohereCompletionOutput Map(
        ICompletionOutput output)
    {
        return output switch
        {
            CohereCompletionOutput cohereCompletionOutput => cohereCompletionOutput,
            OpenAiCompletionOutput openAiCompletionOutput => MapOpenAiCompletionOutput(openAiCompletionOutput),
            TogetherAiCompletionOutput togetherAiCompletionOutput => MapTogetherAiCompletionOutput(togetherAiCompletionOutput),
            AnthropicCompletionOutput anthropicCompletionOutput => MapAnthropicCompletionOutput(anthropicCompletionOutput),
            MistralCompletionOutput mistralCompletionOutput => MapMistralCompletionOutput(mistralCompletionOutput),
            GroqCompletionOutput groqCompletionOutput => MapGroqCompletionOutput(groqCompletionOutput),
            CloudflareCompletionOutput cloudflareCompletionOutput => MapOpenAiCompletionOutput(cloudflareCompletionOutput),
            PerplexityCompletionOutput perplexityCompletionOutput => MapPerplexityCompletionOutput(perplexityCompletionOutput),
            _ => throw new NotSupportedException($"Unsupported output type: {output.GetType().Name}")
        };
    }

    private static CohereCompletionOutput MapOpenAiCompletionOutput(
        OpenAiCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new CohereCompletionOutput
        {
            GenerationId = output.Id,
            Text = choice?.Message?.Content ?? string.Empty,
            FinishReason = choice?.FinishReason,
            Meta = new CohereCompletionMetaOutput
            {
                BilledUnits = new CohereCompletionBilledUnitsMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
                Tokens = new CohereCompletionTokensMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
            }
        };
    }
    
    private static CohereCompletionOutput MapTogetherAiCompletionOutput(
        TogetherAiCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new CohereCompletionOutput
        {
            GenerationId = output.Id,
            Text = choice?.Message?.Content ?? string.Empty,
            FinishReason = choice?.FinishReason,
            Meta = new CohereCompletionMetaOutput
            {
                BilledUnits = new CohereCompletionBilledUnitsMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
                Tokens = new CohereCompletionTokensMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
            }
        };
    }
    
    private static CohereCompletionOutput MapAnthropicCompletionOutput(
        AnthropicCompletionOutput output)
    {
        var textContents = output
            .Content
            .Where(x => x.Type == "text" && !string.IsNullOrWhiteSpace(x.Text))
            .ToList();
        
        var text = string.Join(" ", textContents.Select(x => x.Text));
        
        return new CohereCompletionOutput
        {
            GenerationId = output.Id,
            Text = text,
            FinishReason = output.StopReason,
            Meta = new CohereCompletionMetaOutput
            {
                BilledUnits = new CohereCompletionBilledUnitsMetaOutput
                {
                    InputTokens = output.Usage.InputTokens,
                    OutputTokens = output.Usage.OutputTokens,
                },
                Tokens = new CohereCompletionTokensMetaOutput
                {
                    InputTokens = output.Usage.InputTokens,
                    OutputTokens = output.Usage.OutputTokens,
                },
            }
        };
    }
    
    private static CohereCompletionOutput MapMistralCompletionOutput(
        MistralCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new CohereCompletionOutput
        {
            GenerationId = output.Id,
            Text = choice?.Message?.Content ?? string.Empty,
            FinishReason = choice?.FinishReason,
            Meta = new CohereCompletionMetaOutput
            {
                BilledUnits = new CohereCompletionBilledUnitsMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
                Tokens = new CohereCompletionTokensMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
            }
        };
    }
    
    private static CohereCompletionOutput MapGroqCompletionOutput(
        GroqCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new CohereCompletionOutput
        {
            GenerationId = output.Id,
            Text = choice?.Message?.Content ?? string.Empty,
            FinishReason = choice?.FinishReason,
            Meta = new CohereCompletionMetaOutput
            {
                BilledUnits = new CohereCompletionBilledUnitsMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
                Tokens = new CohereCompletionTokensMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
            }
        };
    }
    
    private static CohereCompletionOutput MapOpenAiCompletionOutput(
        CloudflareCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new CohereCompletionOutput
        {
            GenerationId = output.Id,
            Text = choice?.Message?.Content ?? string.Empty,
            FinishReason = choice?.FinishReason,
            Meta = new CohereCompletionMetaOutput
            {
                BilledUnits = new CohereCompletionBilledUnitsMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
                Tokens = new CohereCompletionTokensMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
            }
        };
    }
    
    private static CohereCompletionOutput MapPerplexityCompletionOutput(
        PerplexityCompletionOutput output)
    {
        var choice = output.Choices.FirstOrDefault();
        return new CohereCompletionOutput
        {
            GenerationId = output.Id,
            Text = choice?.Message?.Content ?? string.Empty,
            FinishReason = choice?.FinishReason,
            Meta = new CohereCompletionMetaOutput
            {
                BilledUnits = new CohereCompletionBilledUnitsMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
                Tokens = new CohereCompletionTokensMetaOutput
                {
                    InputTokens = output.Usage.PromptTokens,
                    OutputTokens = output.Usage.CompletionTokens,
                },
            }
        };
    }
}