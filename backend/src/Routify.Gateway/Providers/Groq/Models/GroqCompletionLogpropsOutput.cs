using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionLogpropsOutput
{
    [JsonPropertyName("content")]
    public List<GroqCompletionLogprobsContentOutput>? Content { get; set; }
}