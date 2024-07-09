using System.Text.Json.Serialization;

namespace Routify.Gateway.Models.OpenAi;

internal record ChatCompletionChoicePayload
{
    [JsonPropertyName("index")]
    public int Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = null!;
    
    [JsonPropertyName("message")]
    public ChatCompletionMessagePayload Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public ChatCompletionLogpropsPayload Logprobs { get; set; } = null!;
}