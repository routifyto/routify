using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Mistral.Models;

internal class MistralCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
    
    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }
    
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}
