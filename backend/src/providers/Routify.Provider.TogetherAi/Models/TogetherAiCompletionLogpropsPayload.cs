using System.Text.Json.Serialization;

namespace Routify.Provider.TogetherAi.Models;

internal record TogetherAiCompletionLogpropsPayload
{
    [JsonPropertyName("tokens")]
    public List<string>? Tokens { get; set; }
    
    [JsonPropertyName("content")]
    public List<float>? TokenLogprobs { get; set; }
}