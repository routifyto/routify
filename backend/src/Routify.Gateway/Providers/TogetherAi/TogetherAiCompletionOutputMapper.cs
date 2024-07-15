using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.OpenAi.Models;
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
}