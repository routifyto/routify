using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionToolInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("function")]
    public TogetherAiCompletionToolFunctionInput? Function { get; set; }
}