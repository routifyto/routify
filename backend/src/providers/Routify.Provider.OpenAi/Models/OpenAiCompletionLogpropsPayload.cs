using System.Text.Json.Serialization;

namespace Routify.Provider.OpenAi.Models;

internal record OpenAiCompletionLogpropsPayload
{
    [JsonPropertyName("content")]
    public List<OpenAiCompletionLogprobsContentPayload>? Content { get; set; }
}