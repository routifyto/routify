using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionBilledUnitsMetaOutput
{
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }
    
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
    
    [JsonPropertyName("search_units")]
    public int SearchUnits { get; set; }
    
    [JsonPropertyName("classifications")]
    public int Classifications { get; set; }
}