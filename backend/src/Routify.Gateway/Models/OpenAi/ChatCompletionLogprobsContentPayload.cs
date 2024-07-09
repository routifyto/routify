using System.Text.Json.Serialization;

namespace Routify.Gateway.Models.OpenAi;

internal record ChatCompletionLogprobsContentPayload
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
    
    [JsonPropertyName("logprob")]
    public float? Logprob { get; set; }
    
    [JsonPropertyName("bytes")]
    public List<byte>? Bytes { get; set; }

    [JsonPropertyName("top_logprobs")] 
    public List<ChatCompletionLogprobsContentPayload> TopLogprobs { get; set; } = [];
}