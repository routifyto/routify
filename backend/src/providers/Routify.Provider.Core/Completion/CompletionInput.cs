namespace Routify.Provider.Core.Completion;

public record CompletionInput
{
    public string? Model { get; set; }
    public List<CompletionMessageInput> Messages { get; set; } = null!;
    public float? TopP { get; set; }
    public int? N { get; set; }
    public string? Stop { get; set; }
    public int? MaxTokens { get; set; }
    public float? PresencePenalty { get; set; }
    public float? FrequencyPenalty { get; set; }
    public double? Temperature { get; set; }
}