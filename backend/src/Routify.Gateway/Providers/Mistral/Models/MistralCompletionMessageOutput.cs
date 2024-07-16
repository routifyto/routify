using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionMessageOutput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}