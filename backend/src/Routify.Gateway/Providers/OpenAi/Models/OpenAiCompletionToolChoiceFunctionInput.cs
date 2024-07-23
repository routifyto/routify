using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionToolChoiceFunctionInput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}