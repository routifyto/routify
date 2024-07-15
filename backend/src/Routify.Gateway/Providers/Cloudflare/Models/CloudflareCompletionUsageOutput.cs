using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cloudflare.Models;

internal record CloudflareCompletionUsageOutput
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }
    
    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }
    
    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}