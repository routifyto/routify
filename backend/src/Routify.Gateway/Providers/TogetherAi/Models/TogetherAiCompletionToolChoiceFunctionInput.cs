using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionToolChoiceFunctionInput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}