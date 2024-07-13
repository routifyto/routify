using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionTokensMetaOutput
{
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }
    
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
}