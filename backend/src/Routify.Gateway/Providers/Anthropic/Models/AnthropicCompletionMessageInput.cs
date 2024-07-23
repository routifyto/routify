using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public AnthropicCompletionMessageContentInput Content { get; set; } = null!;
}