using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cloudflare.Models;

internal record CloudflareCompletionChoiceOutput
{
    [JsonPropertyName("index")]
    public int? Index { get; set; }
    
    [JsonPropertyName("finish_reason")]
    public string? FinishReason { get; set; }
    
    [JsonPropertyName("message")]
    public CloudflareCompletionMessageOutput Message { get; set; } = null!;
    
    [JsonPropertyName("logprobs")]
    public CloudflareCompletionLogpropsOutput? Logprobs { get; set; }
}