using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionMessageContentImageUrlInput
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
    
    [JsonPropertyName("detail")]
    public string? Detail { get; set; }
}