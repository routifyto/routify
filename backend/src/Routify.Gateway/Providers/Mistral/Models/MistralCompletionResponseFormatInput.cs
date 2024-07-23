using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionResponseFormatInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}