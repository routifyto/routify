using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionLogprobsContentOutput
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
    
    [JsonPropertyName("logprob")]
    public float? Logprob { get; set; }
    
    [JsonPropertyName("bytes")]
    public List<byte>? Bytes { get; set; }

    [JsonPropertyName("top_logprobs")] 
    public List<GroqCompletionLogprobsContentOutput> TopLogprobs { get; set; } = [];
}