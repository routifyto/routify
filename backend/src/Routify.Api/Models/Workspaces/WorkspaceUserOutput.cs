namespace Routify.Api.Models.Workspaces;

public record WorkspaceUserOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}