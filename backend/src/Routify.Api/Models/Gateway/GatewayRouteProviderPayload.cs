namespace Routify.Api.Models.Gateway;

public record GatewayRouteProviderPayload
{
    public string Id { get; set; } = null!;
    public string AppProviderId { get; set; } = null!;
    public string? Model { get; set; }
    public Dictionary<string, string>? Attrs { get; set; }
}