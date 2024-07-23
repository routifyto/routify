using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionResponseFormatInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}