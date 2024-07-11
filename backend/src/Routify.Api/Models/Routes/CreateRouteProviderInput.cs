namespace Routify.Api.Models.Routes;

public record CreateRouteProviderInput
{
    public string AppProviderId { get; set; } = null!;
    public string? Model { get; set; }
    public Dictionary<string, string>? Attrs { get; set; }
}
