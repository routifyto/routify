using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Perplexity.Models;

internal record PerplexityCompletionChoiceOutput
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public PerplexityCompletionMessageOutput? Message { get; set; }
    
    [JsonPropertyName("delta")]
    public PerplexityCompletionMessageOutput? Delta { get; set; }
}