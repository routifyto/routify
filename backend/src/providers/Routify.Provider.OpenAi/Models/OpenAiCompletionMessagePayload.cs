using System.Text.Json.Serialization;

namespace Routify.Provider.OpenAi.Models;

internal record OpenAiCompletionMessagePayload
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}