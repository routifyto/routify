using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionCitationOutput
{
    [JsonPropertyName("start")]
    public int Start { get; set; }
    
    [JsonPropertyName("end")]
    public int End { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = null!;

    [JsonPropertyName("document_ids")] 
    public List<string> DocumentIds { get; set; } = [];
}