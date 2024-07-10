using System.Text.Json.Serialization;

namespace Routify.Provider.OpenAi.Models;

internal record OpenAiCompletionChoicePayload
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public OpenAiCompletionMessagePayload Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public OpenAiCompletionLogpropsPayload? Logprobs { get; set; }
}