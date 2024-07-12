using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionMetadataInput
{
    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }
}