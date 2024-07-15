using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cloudflare.Models;

internal record CloudflareCompletionLogpropsOutput
{
    [JsonPropertyName("content")]
    public List<OpenAi.Models.OpenAiCompletionLogprobsContentOutput>? Content { get; set; }
}