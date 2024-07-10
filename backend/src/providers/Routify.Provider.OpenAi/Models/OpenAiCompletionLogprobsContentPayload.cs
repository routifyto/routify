using System.Text.Json.Serialization;

namespace Routify.Provider.OpenAi.Models;

internal record OpenAiCompletionLogprobsContentPayload
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
    
    [JsonPropertyName("logprob")]
    public float? Logprob { get; set; }
    
    [JsonPropertyName("bytes")]
    public List<byte>? Bytes { get; set; }

    [JsonPropertyName("top_logprobs")] 
    public List<OpenAiCompletionLogprobsContentPayload> TopLogprobs { get; set; } = [];
}