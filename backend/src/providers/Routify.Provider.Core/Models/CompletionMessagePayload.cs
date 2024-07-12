namespace Routify.Provider.Core.Models;

public record CompletionMessagePayload
{
    public string Role { get; set; } = null!;
    public string? Content { get; set; }
}