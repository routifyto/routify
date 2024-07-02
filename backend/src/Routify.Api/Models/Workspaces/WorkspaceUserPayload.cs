namespace Routify.Api.Models.Workspaces;

public record WorkspaceUserPayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}