namespace Routify.Api.Models.Routes;

public record UpdateRouteProviderInput
{
    public string? Id { get; set; }
    public string AppProviderId { get; set; } = null!;
    public string? Model { get; set; }
    public Dictionary<string, string>? Attrs { get; set; }
    public int Weight { get; set; }
}