using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionMessageOutput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}