using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionToolInput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("parameter_definitions")]
    public Dictionary<string, CohereCompletionToolParameterInput>? ParameterDefinitions { get; set; }
}