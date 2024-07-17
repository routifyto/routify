using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionLogprobsContentOutput
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = null!;
    
    [JsonPropertyName("logprob")]
    public float? Logprob { get; set; }
    
    [JsonPropertyName("bytes")]
    public List<byte>? Bytes { get; set; }

    [JsonPropertyName("top_logprobs")] 
    public List<AzureOpenAiCompletionLogprobsContentOutput> TopLogprobs { get; set; } = [];
}