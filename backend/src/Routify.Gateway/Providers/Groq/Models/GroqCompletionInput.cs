using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionInput : ICompletionInput
{
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }

    [JsonPropertyName("logit_bias")]
    public Dictionary<string, int>? LogitBias { get; set; }
    
    [JsonPropertyName("logprobs")]
    public bool? Logprobs { get; set; }
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("messages")]
    public List<GroqCompletionMessageInput> Messages { get; set; } = null!;

    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("n")]
    public int? N { get; set; }

    [JsonPropertyName("parallel_tool_calls")]
    public bool? ParallelToolCalls { get; set; }
    
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }

    [JsonPropertyName("response_format")]
    public GroqCompletionResponseFormatInput? ResponseFormat { get; set; }
    
    [JsonPropertyName("seed")]
    public long? Seed { get; set; }

    [JsonPropertyName("stop")]
    public GroqCompletionStopInput? Stop { get; set; }
    
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("stream_options")]
    public GroqCompletionStreamOptionsInput? StreamOptions { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public GroqCompletionToolChoiceInput? ToolChoice { get; set; }

    [JsonPropertyName("tools")]
    public List<GroqCompletionToolInput>? Tools { get; set; }
    
    [JsonPropertyName("top_logprobs")]
    public int? TopLogprobs { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("user")]
    public string? User { get; set; }
}