using System.Text.Json.Serialization;

namespace Routify.Provider.TogetherAi.Models;

internal class TogetherAiCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
}
