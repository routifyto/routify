using Routify.Data.Models;

namespace Routify.Api.Models.Routes;

public record CreateRouteInput
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }

    public Dictionary<string, string> Attrs { get; set; } = [];
    public List<CreateRouteProviderInput> Providers { get; set; } = [];
}