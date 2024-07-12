using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Anthropic.Models;

internal record AnthropicCompletionContentOutput
{
    [JsonPropertyName("type")] 
    public string Type { get; set; } = null!;
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    
    [JsonPropertyName("id")]
    public string? Id { get; set; }
    
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    
    [JsonPropertyName("input")]
    public string? Input { get; set; }
}