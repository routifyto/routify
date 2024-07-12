using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.MistralAi.Models;

internal record MistralAiCompletionMessageOutput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}