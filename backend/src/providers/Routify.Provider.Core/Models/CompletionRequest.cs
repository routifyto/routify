namespace Routify.Provider.Core.Models;

public record CompletionRequest
{
    public CompletionInput Input { get; set; } = null!;
    public int Timeout { get; set; }
    public Dictionary<string, string> AppProviderAttrs { get; set; } = [];
    public Dictionary<string, string> RouteProviderAttrs { get; set; } = [];
    public string? Model { get; set; }
}