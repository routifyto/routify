using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionResponseFormatInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}