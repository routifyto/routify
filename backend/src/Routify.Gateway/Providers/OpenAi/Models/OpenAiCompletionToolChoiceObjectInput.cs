using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionToolChoiceObjectInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("function")]
    public OpenAiCompletionToolChoiceFunctionInput? Function { get; set; }
}