using Routify.Data.Common;
using Routify.Data.Enums;

namespace Routify.Api.Models.Gateway;

public record GatewayRouteOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public string Schema { get; set; } = null!;
    public bool IsLoadBalanceEnabled { get; set; }
    public bool IsFailoverEnabled { get; set; }
    public int? Timeout { get; set; }
    public List<GatewayRouteProviderOutput> Providers { get; set; } = [];
    public CacheConfig? CacheConfig { get; set; }
    public CostLimitConfig? CostLimitConfig { get; set; }
}