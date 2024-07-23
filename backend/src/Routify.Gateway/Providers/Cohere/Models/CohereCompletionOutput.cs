using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionOutput : ICompletionOutput
{
    [JsonPropertyName("text")] 
    public string Text { get; set; } = null!;

    [JsonPropertyName("generation_id")] 
    public string GenerationId { get; set; } = null!;
    
    [JsonPropertyName("citations")]
    public List<CohereCompletionCitationOutput>? Citations { get; set; }
    
    [JsonPropertyName("documents")]
    public List<Dictionary<string, string>>? Documents { get; set; }
    
    [JsonPropertyName("is_search_required")]
    public bool? IsSearchRequired { get; set; }
    
    [JsonPropertyName("search_queries")]
    public List<CohereCompletionSearchQueryOutput>? SearchQueries { get; set; }
    
    [JsonPropertyName("search_results")]
    public List<CohereCompletionSearchResultOutput>? SearchResults { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("tool_calls")]
    public List<CohereCompletionToolCallOutput>? ToolCalls { get; set; }
    
    [JsonPropertyName("chat_history")]
    public List<CohereCompletionMessageOutput>? ChatHistory { get; set; }
    
    [JsonPropertyName("meta")]
    public CohereCompletionMetaOutput? Meta { get; set; }
}