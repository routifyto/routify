namespace Routify.Provider.Core.Completion;

public record CompletionUsagePayload
{
    public int PromptTokens { get; set; }
    public int CompletionTokens { get; set; }
    public int TotalTokens { get; set; }
}