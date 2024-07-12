using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionLogpropsOutput
{
    [JsonPropertyName("content")]
    public List<OpenAiCompletionLogprobsContentOutput>? Content { get; set; }
}