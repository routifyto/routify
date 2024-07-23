using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionChoiceOutput
{
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }

    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("message")]
    public OpenAiCompletionMessageOutput Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public OpenAiCompletionLogpropsOutput? Logprobs { get; set; }
}