using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Perplexity.Models;

internal record PerplexityCompletionMessageOutput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("content")]
    public string? Content { get; set; }
}