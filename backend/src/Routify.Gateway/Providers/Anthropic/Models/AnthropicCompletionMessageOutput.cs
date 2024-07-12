using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionMessageOutput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}