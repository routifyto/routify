using System.Text.Json.Serialization;

namespace Routify.Gateway.Models.OpenAi;

internal record ChatCompletionInput
{
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("messages")]
    public List<ChatCompletionMessageInput> Messages { get; set; } = null!;
    
    [JsonPropertyName("top_p")]
    public float? TopP { get; set; }
    
    [JsonPropertyName("n")]
    public int? N { get; set; }
    
    [JsonPropertyName("stop")]
    public string? Stop { get; set; }
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }
    
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }
    
    [JsonPropertyName("temperature")]
    public int? Temperature { get; set; }
}