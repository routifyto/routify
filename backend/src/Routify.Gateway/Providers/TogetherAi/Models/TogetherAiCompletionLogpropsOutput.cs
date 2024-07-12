using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.TogetherAi.Models;

internal record TogetherAiCompletionLogpropsOutput
{
    [JsonPropertyName("tokens")]
    public List<string>? Tokens { get; set; }
    
    [JsonPropertyName("content")]
    public List<float>? TokenLogprobs { get; set; }
}