using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionInput : ICompletionInput
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<MistralCompletionMessageInput> Messages { get; set; } = null!;
    
    [JsonPropertyName("response_format")]
    public MistralCompletionResponseFormatInput? ResponseFormat { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
    
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }

    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("safe_prompt")]
    public bool? SafePrompt { get; set; }
    
    [JsonPropertyName("tools")]
    public List<MistralCompletionToolInput>? Tools { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public string? ToolChoice { get; set; }
    
    [JsonPropertyName("random_seed")]
    public long? RandomSeed { get; set; }
}