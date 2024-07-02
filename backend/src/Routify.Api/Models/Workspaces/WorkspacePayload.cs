namespace Routify.Api.Models.Workspaces;

public record WorkspacePayload
{
    public WorkspaceUser User { get; set; } = null!;
}