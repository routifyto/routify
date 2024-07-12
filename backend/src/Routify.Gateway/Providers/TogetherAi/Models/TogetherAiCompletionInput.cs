using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionInput : ICompletionInput
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<TogetherAiCompletionMessageInput> Messages { get; set; } = null!;
    
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("top_k")]
    public int? TopK { get; set; }
    
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
    
    [JsonPropertyName("repetition_penalty")]
    public float? RepetitionPenalty { get; set; }
    
    [JsonPropertyName("temperature")]
    public double? Temperature { get; set; }
    
    [JsonPropertyName("logprobs")]
    public int? Logprobs { get; set; }
    
    [JsonPropertyName("echo")]
    public bool? Echo { get; set; }
    
    [JsonPropertyName("logit_bias")]
    public Dictionary<string, float>? LogitBias { get; set; }
}