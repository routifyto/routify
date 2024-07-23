using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Groq.Models;

internal record GroqCompletionOutput : ICompletionOutput
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;
    
    [JsonPropertyName("created")]
    public long Created { get; set; }
    
    [JsonPropertyName("system_fingerprint")]
    public string? SystemFingerprint { get; set; }

    [JsonPropertyName("choices")]
    public List<GroqCompletionChoiceOutput> Choices { get; set; } = null!;
    
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;
    
    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }
    
    [JsonPropertyName("usage")]
    public GroqCompletionUsageOutput Usage { get; set; } = null!;
}