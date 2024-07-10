namespace Routify.Provider.Core.Completion;

public class CompletionMessageInput
{
    public string Role { get; set; } = null!;
    public string Content { get; set; } = null!;
    public string? Name { get; set; }
}
