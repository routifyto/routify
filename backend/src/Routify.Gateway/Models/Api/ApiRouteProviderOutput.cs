namespace Routify.Gateway.Models.Api;

internal record ApiRouteProviderOutput
{
    public string Id { get; set; } = null!;
    public string AppProviderId { get; set; } = null!;
    public string? Model { get; set; }
    public Dictionary<string, string> Attrs { get; set; } = [];
    public int Weight { get; set; }
}