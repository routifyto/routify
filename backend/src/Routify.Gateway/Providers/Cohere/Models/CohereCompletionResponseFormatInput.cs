using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionResponseFormatInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("schema")]
    public JsonObject? Schema { get; set; }
}