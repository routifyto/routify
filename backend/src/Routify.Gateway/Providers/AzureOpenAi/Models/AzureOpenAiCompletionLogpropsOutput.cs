using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionLogpropsOutput
{
    [JsonPropertyName("content")]
    public List<AzureOpenAiCompletionLogprobsContentOutput>? Content { get; set; }
}