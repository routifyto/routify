using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionMessageToolCallInput
{
    [JsonPropertyName("function")]
    public GroqCompletionMessageToolCallFunctionInput? Function { get; set; }
    
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}