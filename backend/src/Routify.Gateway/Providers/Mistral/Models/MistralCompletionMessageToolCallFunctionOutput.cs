using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionMessageToolCallFunctionOutput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("arguments")]
    public JsonObject? Arguments { get; set; }
}