using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionChoiceOutput
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public AzureOpenAiCompletionMessageOutput Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public AzureOpenAiCompletionLogpropsOutput? Logprobs { get; set; }
}