using System.Text.Json.Serialization;

namespace Routify.Provider.TogetherAi.Models;

internal record TogetherAiCompletionChoicePayload
{
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public TogetherAiCompletionMessagePayload Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public TogetherAiCompletionLogpropsPayload? Logprobs { get; set; }
}