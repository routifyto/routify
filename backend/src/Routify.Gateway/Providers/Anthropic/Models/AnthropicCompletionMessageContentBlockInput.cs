using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionMessageContentBlockInput
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;
    
    [JsonPropertyName("source")]
    public AnthropicCompletionMessageSourceInput Source { get; set; } = null!;
}