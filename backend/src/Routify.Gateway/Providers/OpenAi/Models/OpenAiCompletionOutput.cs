using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.OpenAi.Models;

internal record OpenAiCompletionOutput : ICompletionOutput
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("choices")]
    public List<OpenAiCompletionChoiceOutput> Choices { get; set; } = null!;
    
    [JsonPropertyName("created")]
    public long Created { get; set; }
    
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;
    
    [JsonPropertyName("service_tier")]
    public string? ServiceTier { get; set; }
    
    [JsonPropertyName("system_fingerprint")]
    public string? SystemFingerprint { get; set; }

    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;
    
    [JsonPropertyName("usage")]
    public OpenAiCompletionUsageOutput Usage { get; set; } = null!;
}