using Routify.Gateway.Models.Data;
using Routify.Gateway.Services;
using RouteData = Routify.Gateway.Models.Data.RouteData;

namespace Routify.Gateway.Abstractions;

internal record RequestContext
{
    public HttpContext HttpContext { get; set; } = null!;
    public AppData App { get; set; } = null!;
    public RouteData Route { get; set; } = null!;
    public ApiKeyData ApiKey { get; set; } = null!;
    public ConsumerData? Consumer { get; set; }
    public CacheService Cache { get; set; } = null!;
}