using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionStreamOptionsInput
{
    [JsonPropertyName("include_usage")]
    public bool? IncludeUsage { get; set; }
}