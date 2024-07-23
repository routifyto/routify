using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionToolChoiceObjectInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("function")]
    public TogetherAiCompletionToolChoiceFunctionInput? Function { get; set; }
}