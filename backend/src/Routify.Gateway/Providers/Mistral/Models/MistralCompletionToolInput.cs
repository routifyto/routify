using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionToolInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
    
    [JsonPropertyName("function")]
    public MistralCompletionToolFunctionInput? Function { get; set; }
}