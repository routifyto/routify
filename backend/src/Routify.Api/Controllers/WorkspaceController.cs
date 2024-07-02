using Microsoft.AspNetCore.Mvc;
using Routify.Api.Models.Workspaces;
using Routify.Data;

namespace Routify.Api.Controllers;

[Route("v1/workspace")]
public class WorkspaceController(
    DatabaseContext databaseContext) 
    : BaseController
{
    [HttpGet(Name = "GetWorkspace")]
    public async Task<ActionResult<WorkspacePayload>> GetWorkspaceAsync(
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var user = await databaseContext
            .Users
            .FindAsync(CurrentUserId, cancellationToken);

        if (user is null)
            return NotFound();

        var payload = new WorkspacePayload
        {
            User = new WorkspaceUser
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            }
        };

        return Ok(payload);
    }
}