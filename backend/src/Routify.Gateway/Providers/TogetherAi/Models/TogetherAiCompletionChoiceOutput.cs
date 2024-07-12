using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionChoiceOutput
{
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public TogetherAiCompletionMessageOutput Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public TogetherAiCompletionLogpropsOutput? Logprobs { get; set; }
}