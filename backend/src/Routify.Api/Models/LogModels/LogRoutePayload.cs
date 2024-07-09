namespace Routify.Api.Models.LogModels;

public class LogRoutePayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string Path { get; set; } = null!;
}