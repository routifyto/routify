using Routify.Data.Models;

namespace Routify.Api.Models.Routes;

public record RoutePayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    
    public Dictionary<string, string> Attrs { get; set; } = [];
    
    public List<RouteProviderPayload> Providers { get; set; } = [];
}