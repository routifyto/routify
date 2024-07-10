using System.Text.Json.Serialization;

namespace Routify.Provider.OpenAi.Models;

internal record OpenAiCompletionPayload
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;
    
    [JsonPropertyName("choices")]
    public List<OpenAiCompletionChoicePayload> Choices { get; set; } = null!;
    
    [JsonPropertyName("created")]
    public long Created { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;
    
    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }
    
    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; set; } = null!;
    
    [JsonPropertyName("usage")]
    public OpenAiCompletionUsagePayload Usage { get; set; } = null!;
}