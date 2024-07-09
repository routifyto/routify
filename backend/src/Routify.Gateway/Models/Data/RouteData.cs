using Routify.Data.Models;

namespace Routify.Gateway.Models.Data;

internal record RouteData
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public RouteInputType InputType { get; set; }
    public List<RouteProviderData> Providers { get; set; } = [];
}