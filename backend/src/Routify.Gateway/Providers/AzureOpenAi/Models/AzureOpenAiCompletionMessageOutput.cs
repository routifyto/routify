using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionMessageOutput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}