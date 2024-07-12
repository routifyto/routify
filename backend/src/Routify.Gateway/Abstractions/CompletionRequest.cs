namespace Routify.Gateway.Abstractions;

internal record CompletionRequest
{
    public ICompletionInput Input { get; set; } = null!;
    public int Timeout { get; set; }
    public Dictionary<string, string> AppProviderAttrs { get; set; } = [];
    public Dictionary<string, string> RouteProviderAttrs { get; set; } = [];
    public string? Model { get; set; }
}