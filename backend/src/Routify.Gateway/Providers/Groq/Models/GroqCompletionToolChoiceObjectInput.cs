using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionToolChoiceObjectInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("function")]
    public GroqCompletionToolChoiceFunctionInput? Function { get; set; }
}