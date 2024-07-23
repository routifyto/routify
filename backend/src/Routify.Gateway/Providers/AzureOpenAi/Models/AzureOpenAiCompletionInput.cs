using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionInput : ICompletionInput
{
    [JsonPropertyName("messages")]
    public List<AzureOpenAiCompletionMessageInput> Messages { get; set; } = null!;
    
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
    public AzureOpenAiCompletionResponseFormatInput? ResponseFormat { get; set; }
    
    [JsonPropertyName("seed")]
    public long? Seed { get; set; }

    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }
    
    [JsonPropertyName("stop")]
    public AzureOpenAiCompletionStopInput? Stop { get; set; }
    
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("stream_options")]
    public AzureOpenAiCompletionStreamOptionsInput? StreamOptions { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }

    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("tools")]
    public List<AzureOpenAiCompletionToolInput>? Tools { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public AzureOpenAiCompletionToolChoiceInput? ToolChoice { get; set; }
    
    [JsonPropertyName("parallel_tool_calls")]
    public bool? ParallelToolCalls { get; set; }
    
    [JsonPropertyName("user")]
    public string? User { get; set; }
}