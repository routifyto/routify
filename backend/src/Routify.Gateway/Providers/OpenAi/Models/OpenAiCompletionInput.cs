using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionInput : ICompletionInput
{
    [JsonPropertyName("messages")]
    public List<OpenAiCompletionMessageInput> Messages { get; set; } = null!;
    
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }
    
    [JsonPropertyName("logit_bias")]
    public Dictionary<int, int>? LogitBias { get; set; }
    
    [JsonPropertyName("logprobs")]
    public bool? Logprobs { get; set; }
    
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs { get; set; }
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    
    [JsonPropertyName("n")]
    public int? N { get; set; }

    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }

    [JsonPropertyName("response_format")]
    public OpenAiCompletionResponseFormatInput? ResponseFormat { get; set; }
    
    [JsonPropertyName("seed")]
    public long? Seed { get; set; }

    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }
    
    [JsonPropertyName("stop")]
    public OpenAiCompletionStopInput? Stop { get; set; }
    
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("stream_options")]
    public OpenAiCompletionStreamOptionsInput? StreamOptions { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("tools")]
    public List<OpenAiCompletionToolInput>? Tools { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public OpenAiCompletionToolChoiceInput? ToolChoice { get; set; }
    
    [JsonPropertyName("parallel_tool_calls")]
    public bool? ParallelToolCalls { get; set; }
    
    [JsonPropertyName("user")]
    public string? User { get; set; }
    
}