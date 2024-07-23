using System.Text.Json.Serialization;
using Routify.Core.Models;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Cohere.Models;

internal record CohereCompletionInput : ICompletionInput
{
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
    [JsonPropertyName("model")]
    public string? Model { get; set; }
    
    [JsonPropertyName("stream")]
    public bool? Stream { get; set; }
    
    [JsonPropertyName("preamble")]
    public string? Preamble { get; set; }
    
    [JsonPropertyName("chat_history")]
    public List<CohereCompletionMessageInput> ChatHistory { get; set; } = null!;
    
    [JsonPropertyName("conversation_id")]
    public string? ConversationId { get; set; }
    
    [JsonPropertyName("prompt_truncation")]
    public string? PromptTruncation { get; set; }
    
    [JsonPropertyName("connectors")]
    public List<CohereCompletionConnectorInput>? Connectors { get; set; }
    
    [JsonPropertyName("search_queries_only")]
    public bool? SearchQueriesOnly { get; set; }
    
    [JsonPropertyName("documents")]
    public List<JsonObject>? Documents { get; set; }
    
    [JsonPropertyName("citation_quality")]
    public string? CitationQuality { get; set; }
    
    [JsonPropertyName("temperature")]
    public float? Temperature { get; set; }
    
    [JsonPropertyName("max_tokens")]
    public int? MaxTokens { get; set; }
    
    [JsonPropertyName("max_input_tokens")]
    public int? MaxInputTokens { get; set; }
    
    [JsonPropertyName("k")]
    public int? K { get; set; }
    
    [JsonPropertyName("p")]
    public float? P { get; set; }
    
    [JsonPropertyName("seed")]
    public long? Seed { get; set; }
    
    [JsonPropertyName("stop_sequences")]
    public List<string>? StopSequences { get; set; }
    
    [JsonPropertyName("frequency_penalty")]
    public float? FrequencyPenalty { get; set; }
    
    [JsonPropertyName("presence_penalty")]
    public float? PresencePenalty { get; set; }
    
    [JsonPropertyName("tools")]
    public List<CohereCompletionToolInput>? Tools { get; set; }
    
    [JsonPropertyName("tool_results")]
    public List<JsonObject>? ToolResults { get; set; }
    
    [JsonPropertyName("force_single_step")]
    public bool? ForceSingleStep { get; set; }
    
    [JsonPropertyName("response_format")]
    public CohereCompletionResponseFormatInput? ResponseFormat { get; set; }
}