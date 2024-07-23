using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Mistral.Models;

internal record MistralCompletionOutput : ICompletionOutput
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("object")]
    public string Object { get; set; } = null!;
    
    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;

    [JsonPropertyName("choices")]
    public List<MistralCompletionChoiceOutput> Choices { get; set; } = null!;
    
    [JsonPropertyName("usage")]
    public MistralCompletionUsageOutput Usage { get; set; } = null!;
}