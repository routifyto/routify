using System.Text.Json.Serialization;

namespace Routify.Gateway.Models.OpenAi;

internal class ChatCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
