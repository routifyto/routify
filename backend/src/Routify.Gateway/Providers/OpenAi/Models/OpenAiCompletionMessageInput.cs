using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal class OpenAiCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public OpenAiCompletionMessageContentInput Content { get; set; } = null!;
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("tool_call_id")]
    public string? ToolCallId { get; set; }
}
