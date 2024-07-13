using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionMessageOutput
{
    [JsonPropertyName("role")] 
    public string Role { get; set; } = null!;

    [JsonPropertyName("message")] 
    public string Message { get; set; } = null!;
}