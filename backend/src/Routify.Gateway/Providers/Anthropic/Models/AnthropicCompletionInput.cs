using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionInput : ICompletionInput
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<AnthropicCompletionMessageInput> Messages { get; set; } = null!;
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    
    [JsonPropertyName("metadata")]
    public AnthropicCompletionMetadataInput? Metadata { get; set; }
    
    [JsonPropertyName("stop_sequences")]
    public List<string>? StopSequences { get; set; }
    
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("system")]
    public string? System { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
    
    [JsonPropertyName("tool_choice")]
    public AnthropicCompletionToolChoiceInput? ToolChoice { get; set; }
    
    [JsonPropertyName("tools")]
    public List<AnthropicCompletionToolInput>? Tools { get; set; }
    
    [JsonPropertyName("top_k")]
    public int? TopK { get; set; }
    
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
}