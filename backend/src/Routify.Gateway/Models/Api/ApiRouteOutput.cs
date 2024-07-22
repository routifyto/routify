using Routify.Data.Common;
using Routify.Data.Enums;

namespace Routify.Gateway.Models.Api;

internal record ApiRouteOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public string Schema { get; set; } = null!;
    public bool IsLoadBalanceEnabled { get; set; }
    public bool IsFailoverEnabled { get; set; }
    public int? Timeout { get; set; }
    public List<ApiRouteProviderOutput> Providers { get; set; } = [];
    public CacheConfig? CacheConfig { get; set; }
    public CostLimitConfig? CostLimitConfig { get; set; }
}