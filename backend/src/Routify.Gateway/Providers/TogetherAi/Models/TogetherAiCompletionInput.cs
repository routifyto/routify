using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionInput : ICompletionInput
{
    [JsonPropertyName("messages")]
    public List<TogetherAiCompletionMessageInput> Messages { get; set; } = null!;

    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("stop")]
    public List<string>? Stop { get; set; }

    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("top_k")]
    public int? TopK { get; set; }
    
    [JsonPropertyName("repetition_penalty")]
    public float? RepetitionPenalty { get; set; }

    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("logprobs")]
    public int? Logprobs { get; set; }

    [JsonPropertyName("echo")]
    public bool? Echo { get; set; }

    [JsonPropertyName("n")]
    public int? N { get; set; }
    
    [JsonPropertyName("min_p")]
    public float? MinP { get; set; }
    
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }
    
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }
    
    [JsonPropertyName("logit_bias")]
    public Dictionary<string, float>? LogitBias { get; set; }
    
    [JsonPropertyName("response_format")]
    public TogetherAiCompletionResponseFormatInput? ResponseFormat { get; set; }
    
    [JsonPropertyName("tools")]
    public List<TogetherAiCompletionToolInput>? Tools { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public TogetherAiCompletionToolChoiceInput? ToolChoice { get; set; }
    
    [JsonPropertyName("safety_model")]
    public string? SafetyModel { get; set; }
    
}