namespace Routify.Provider.Core.Completion;

public record CompletionMessagePayload
{
    public string Role { get; set; } = null!;
    public string? Content { get; set; }
}