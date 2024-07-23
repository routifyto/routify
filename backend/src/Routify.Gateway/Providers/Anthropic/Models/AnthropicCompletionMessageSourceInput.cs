using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionMessageSourceInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("media_type")]
    public string? MediaType { get; set; }
    
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}