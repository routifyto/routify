using System.Text.Json.Serialization;

namespace Routify.Gateway.Models.OpenAi;

internal record ChatCompletionLogpropsPayload
{
    [JsonPropertyName("content")]
    public List<ChatCompletionLogprobsContentPayload> Content { get; set; } = [];
}