using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionConnectorOutput
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}