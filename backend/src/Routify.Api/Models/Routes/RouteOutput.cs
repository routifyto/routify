using Routify.Data.Enums;

namespace Routify.Api.Models.Routes;

public record RouteOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public string Schema { get; set; } = null!;
    public RouteStrategy Strategy { get; set; }
    
    public Dictionary<string, string> Attrs { get; set; } = [];
    
    public List<RouteProviderOutput> Providers { get; set; } = [];
}