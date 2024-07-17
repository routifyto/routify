using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionInput : ICompletionInput
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<AzureOpenAiCompletionMessageInput> Messages { get; set; } = null!;
    
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("n")]
    public int? N { get; set; }
    
    [JsonPropertyName("stop")]
    public string? Stop { get; set; }
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }
    
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
    
    [JsonPropertyName("seed")]
    public long? Seed { get; set; }
    
    [JsonPropertyName("logprobs")]
    public bool? Logprobs { get; set; }
    
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs { get; set; }
    
    [JsonPropertyName("user")]
    public string? User { get; set; }
}