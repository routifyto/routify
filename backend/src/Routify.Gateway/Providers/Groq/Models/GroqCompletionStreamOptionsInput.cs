using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionStreamOptionsInput
{
    [JsonPropertyName("include_usage")]
    public bool? IncludeUsage { get; set; }
}