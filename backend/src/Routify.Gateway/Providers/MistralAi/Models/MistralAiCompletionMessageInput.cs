using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.MistralAi.Models;

internal class MistralAiCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
    
    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }
}
