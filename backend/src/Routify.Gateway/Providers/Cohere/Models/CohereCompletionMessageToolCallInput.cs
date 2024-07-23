using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.Cohere.Models;

internal class CohereCompletionMessageToolCallInput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("parameters")]
    public JsonObject? Parameters { get; set; }
}