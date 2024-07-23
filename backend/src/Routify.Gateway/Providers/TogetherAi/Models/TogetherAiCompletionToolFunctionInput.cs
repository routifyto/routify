using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionToolFunctionInput
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("parameters")]
    public JsonObject? Parameters { get; set; }
}