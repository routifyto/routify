using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionMessageOutput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}