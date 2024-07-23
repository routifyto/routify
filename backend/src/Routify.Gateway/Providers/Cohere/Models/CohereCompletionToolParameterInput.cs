using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionToolParameterInput
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("required")]
    public bool? Required { get; set; }
}