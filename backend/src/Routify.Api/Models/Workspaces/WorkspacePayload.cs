namespace Routify.Api.Models.Workspaces;

public record WorkspacePayload
{
    public WorkspaceUserPayload User { get; set; } = null!;
    public List<WorkspaceAppPayload> Apps { get; set; } = [];
}