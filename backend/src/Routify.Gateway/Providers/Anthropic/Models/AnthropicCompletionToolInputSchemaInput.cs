using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionToolInputSchemaInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("properties")]
    public JsonObject? Properties { get; set; }
}