using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record OpenAiCompletionToolChoiceFunctionInput
{
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}