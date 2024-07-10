namespace Routify.Provider.Core.Completion;

public record CompletionRequest
{
    public CompletionInput Input { get; set; } = null!;
    public int Timeout { get; set; }
    public Dictionary<string, string> ProviderAttrs { get; set; } = [];
}