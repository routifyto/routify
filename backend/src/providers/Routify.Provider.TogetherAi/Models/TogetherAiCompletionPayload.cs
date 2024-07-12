using System.Text.Json.Serialization;

namespace Routify.Provider.TogetherAi.Models;

internal record TogetherAiCompletionPayload
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;
    
    [JsonPropertyName("choices")]
    public List<TogetherAiCompletionChoicePayload> Choices { get; set; } = null!;
    
    [JsonPropertyName("created")]
    public long Created { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;
    
    [JsonPropertyName("usage")]
    public TogetherAiCompletionUsagePayload Usage { get; set; } = null!;
}