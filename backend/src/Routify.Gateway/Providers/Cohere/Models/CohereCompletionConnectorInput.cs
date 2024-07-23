using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionConnectorInput
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("user_access_token")]
    public string? UserAccessToken { get; set; }
    
    [JsonPropertyName("continue_on_failure")]
    public bool? ContinueOnFailure { get; set; }

    [JsonPropertyName("options")] 
    public JsonObject? Options { get; set; }
}