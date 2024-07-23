using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionResponseFormatInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("schema")]
    public JsonObject? Schema { get; set; }
}