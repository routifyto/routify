using System.Text.Json.Serialization;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionOutput : ICompletionOutput
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("role")] 
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public List<AnthropicCompletionContentOutput> Content { get; set; } = [];
    
    [JsonPropertyName("model")]
    public string Model { get; set; } = null!;
    
    [JsonPropertyName("stop_reason")]
    public string? StopReason { get; set; }
    
    [JsonPropertyName("stop_sequence")]
    public string? StopSequence { get; set; }
    
    [JsonPropertyName("usage")]
    public AnthropicCompletionUsageOutput Usage { get; set; } = null!;
}