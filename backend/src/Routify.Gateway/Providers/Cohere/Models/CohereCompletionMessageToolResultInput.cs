using System.Text.Json.Serialization;
using Routify.Core.Models;

namespace Routify.Gateway.Providers.Cohere.Models;

internal class CohereCompletionMessageToolResultInput
{
    [JsonPropertyName("call")]
    public CohereCompletionMessageToolCallInput? Call { get; set; }
    
    [JsonPropertyName("outputs")]
    public List<JsonObject>? Outputs { get; set; }
}