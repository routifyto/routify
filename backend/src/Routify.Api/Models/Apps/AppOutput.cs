using Routify.Data.Enums;

namespace Routify.Api.Models.Apps;

public record AppOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public AppRole Role { get; set; }
}