using Routify.Data.Models;

namespace Routify.Gateway.Models.Api;

internal record ApiRoutePayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public RouteInputType InputType { get; set; }
    public List<ApiRouteProviderPayload> Providers { get; set; } = [];
}