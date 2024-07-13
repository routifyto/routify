using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionApiMetaOutput
{
    [JsonPropertyName("version")]
    public string Version { get; set; } = null!;
    
    [JsonPropertyName("is_deprecated")]
    public bool IsDeprecated { get; set; }
    
    [JsonPropertyName("is_experimental")]
    public bool IsExperimental { get; set; }
}