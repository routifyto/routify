using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.MistralAi.Models;

internal record MistralAiCompletionChoiceOutput
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public MistralAiCompletionMessageOutput Message { get; set; } = null!;
}