using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionToolResultOutput
{
    [JsonPropertyName("call")]
    public CohereCompletionToolCallOutput? Call { get; set; }
    
    [JsonPropertyName("outputs")]
    public List<CohereCompletionMessageOutput>? Outputs { get; set; }
}