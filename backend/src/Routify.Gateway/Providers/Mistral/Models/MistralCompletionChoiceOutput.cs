using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionChoiceOutput
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public MistralCompletionMessageOutput Message { get; set; } = null!;
}