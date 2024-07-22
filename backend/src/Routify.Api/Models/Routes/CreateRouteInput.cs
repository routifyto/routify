using Routify.Data.Common;
using Routify.Data.Enums;

namespace Routify.Api.Models.Routes;

public record CreateRouteInput
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public string Schema { get; set; } = null!;
    
    public bool IsLoadBalanceEnabled { get; set; }
    public bool IsFailoverEnabled { get; set; }
    public int? Timeout { get; set; }

    public Dictionary<string, string> Attrs { get; set; } = [];
    public List<CreateRouteProviderInput> Providers { get; set; } = [];
    
    public CacheConfig? CacheConfig { get; set; }
    public CostLimitConfig? CostLimitConfig { get; set; }
}