using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cloudflare.Models;

internal record CloudflareCompletionLogprobsContentOutput
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
    
    [JsonPropertyName("logprob")]
    public float? Logprob { get; set; }
    
    [JsonPropertyName("bytes")]
    public List<byte>? Bytes { get; set; }

    [JsonPropertyName("top_logprobs")] 
    public List<CloudflareCompletionLogprobsContentOutput> TopLogprobs { get; set; } = [];
}