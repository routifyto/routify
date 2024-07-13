using Routify.Data.Models;

namespace Routify.Api.Models.Gateway;

public record GatewayRouteOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public string Schema { get; set; } = null!;
    public List<GatewayRouteProviderOutput> Providers { get; set; } = [];
}