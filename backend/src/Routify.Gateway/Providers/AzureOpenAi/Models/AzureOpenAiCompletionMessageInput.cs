using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal class AzureOpenAiCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string Content { get; set; } = null!;
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}
