using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionSearchResultOutput
{
    [JsonPropertyName("search_query")]
    public CohereCompletionSearchQueryOutput? SearchQuery { get; set; }
    
    [JsonPropertyName("connector")]
    public CohereCompletionConnectorOutput? Connector { get; set; }
    
    [JsonPropertyName("document_ids")]
    public List<string>? DocumentIds { get; set; }
    
    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; set; }
    
    [JsonPropertyName("continue_on_failure")]
    public bool? ContinueOnFailure { get; set; }
}