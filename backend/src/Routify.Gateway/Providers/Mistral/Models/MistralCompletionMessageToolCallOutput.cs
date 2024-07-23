using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionMessageToolCallOutput
{
    [JsonPropertyName("function")]
    public MistralCompletionMessageToolCallFunctionOutput? Function { get; set; }
}