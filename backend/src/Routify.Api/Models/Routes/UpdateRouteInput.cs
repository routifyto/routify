using Routify.Data.Common;

namespace Routify.Api.Models.Routes;

public record UpdateRouteInput
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Path { get; set; } = null!;
    public string Schema { get; set; } = null!;
    
    public bool IsLoadBalanceEnabled { get; set; }
    public bool IsFailoverEnabled { get; set; }
    public int? Timeout { get; set; }
    
    public Dictionary<string, string> Attrs { get; set; } = [];
    public List<UpdateRouteProviderInput> Providers { get; set; } = [];
    
    public CacheConfig? CacheConfig { get; set; }
    public CostLimitConfig? CostLimitConfig { get; set; }
}