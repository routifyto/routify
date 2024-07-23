namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionStreamOptionsInput
{
    public bool? IncludeUsage { get; set; }
}