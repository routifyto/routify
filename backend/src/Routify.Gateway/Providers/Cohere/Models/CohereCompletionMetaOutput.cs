using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionMetaOutput
{
    [JsonPropertyName("api_version")]
    public CohereCompletionApiMetaOutput? ApiVersion { get; set; }
    
    [JsonPropertyName("billed_units")]
    public CohereCompletionBilledUnitsMetaOutput? BilledUnits { get; set; }
    
    [JsonPropertyName("tokens")]
    public CohereCompletionTokensMetaOutput? Tokens { get; set; }
    
    [JsonPropertyName("warnings")]
    public List<string>? Warnings { get; set; }
}