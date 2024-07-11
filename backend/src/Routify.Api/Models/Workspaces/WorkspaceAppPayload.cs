using Routify.Data.Models;

namespace Routify.Api.Models.Workspaces;

public record WorkspaceAppPayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public AppUserRole Role { get; set; }
}