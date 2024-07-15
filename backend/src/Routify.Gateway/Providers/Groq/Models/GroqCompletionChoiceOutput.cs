using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionChoiceOutput
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public GroqCompletionMessageOutput Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public GroqCompletionLogpropsOutput? Logprobs { get; set; }
}