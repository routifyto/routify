using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionResponseFormatInput
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }
}