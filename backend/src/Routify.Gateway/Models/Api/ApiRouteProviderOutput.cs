namespace Routify.Gateway.Models.Api;

internal record ApiRouteProviderOutput
{
    public string Id { get; set; } = null!;
    public string AppProviderId { get; set; } = null!;
    public int Index { get; set; }
    public string? Model { get; set; }
    public Dictionary<string, string> Attrs { get; set; } = [];
    public int Weight { get; set; }
    public int? Timeout { get; set; }
}