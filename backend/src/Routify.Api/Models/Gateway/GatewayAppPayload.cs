namespace Routify.Api.Models.Gateway;

public record GatewayAppPayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<GatewayRoutePayload> Routes { get; set; } = [];
    public List<GatewayAppProviderPayload> Providers { get; set; } = [];
}