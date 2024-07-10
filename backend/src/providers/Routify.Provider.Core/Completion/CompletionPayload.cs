namespace Routify.Provider.Core.Completion;

public record CompletionPayload
{
    public string Id { get; set; } = null!;
    public string Object { get; set; } = null!;
    public List<CompletionChoicePayload> Choices { get; set; } = null!;
    public long Created { get; set; }
    public string Model { get; set; } = null!;
    public string? ServiceTier { get; set; }
    public string SystemFingerprint { get; set; } = null!;
    public CompletionUsagePayload Usage { get; set; } = null!;
}