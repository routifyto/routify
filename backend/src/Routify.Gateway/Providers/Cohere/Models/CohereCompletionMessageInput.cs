using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal class CohereCompletionMessageInput
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = null!;
    
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;
}
