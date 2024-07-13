namespace Routify.Api.Models.Workspaces;

public record WorkspaceOutput
{
    public WorkspaceUserOutput User { get; set; } = null!;
    public List<WorkspaceAppOutput> Apps { get; set; } = [];
}