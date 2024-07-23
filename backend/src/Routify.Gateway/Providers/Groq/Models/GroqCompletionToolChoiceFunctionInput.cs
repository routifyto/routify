using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionToolChoiceFunctionInput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}