using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionToolInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("function")]
    public OpenAiCompletionToolFunctionInput? Function { get; set; }
}