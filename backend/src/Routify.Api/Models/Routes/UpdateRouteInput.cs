namespace Routify.Api.Models.Routes;

public record UpdateRouteInput
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Path { get; set; } = null!;
    public Dictionary<string, string> Attrs { get; set; } = [];
    public List<UpdateRouteProviderInput> Providers { get; set; } = [];
}