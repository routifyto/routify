using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionToolFunctionInput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("parameters")]
    public JsonObject? Parameters { get; set; }
}