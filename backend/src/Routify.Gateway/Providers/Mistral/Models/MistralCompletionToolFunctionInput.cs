using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionToolFunctionInput
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("parameters")]
    public JsonObject? Parameters { get; set; }
}