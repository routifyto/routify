using System.Text.Json.Serialization;
using Routify.Gateway.Providers.Anthropic.Converters;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    [JsonConverter(typeof(AnthropicMessageContentConverter))]
    public AnthropicCompletionMessageContentInput Content { get; set; } = null!;
}