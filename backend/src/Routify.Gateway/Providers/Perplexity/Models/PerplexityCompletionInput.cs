using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Perplexity.Models;

internal record PerplexityCompletionInput : ICompletionInput
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<PerplexityCompletionMessageInput> Messages { get; set; } = null!;
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
    
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("return_citations")]
    public bool? ReturnCitations { get; set; }
    
    [JsonPropertyName("return_images")]
    public bool? ReturnImages { get; set; }
    
    [JsonPropertyName("top_k")]
    public int? TopK { get; set; }
    
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }
    
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }
}