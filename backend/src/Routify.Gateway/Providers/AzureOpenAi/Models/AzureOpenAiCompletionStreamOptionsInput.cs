using System.Text.Json.Serialization;

namespace Routify.Gateway.Providers.AzureOpenAi.Models;

internal record AzureOpenAiCompletionStreamOptionsInput
{
    [JsonPropertyName("include_usage")]
    public bool? IncludeUsage { get; set; }
}