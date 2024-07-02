using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        var userApps = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .Where(x => x.UserId == user.Id)
            .ToListAsync(cancellationToken);
        
        var payload = new WorkspacePayload
        {
            User = new WorkspaceUserPayload
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            },
            Apps = []
        };

        foreach (var userApp in userApps)
        {
            var app = userApp.App;
            if (app == null)
                continue;

            var appPayload = new WorkspaceAppPayload
            {
                Id = app.Id,
                Name = app.Name,
                Role = userApp.Role
            };
            
            payload.Apps.Add(appPayload);
        }
        
        return Ok(payload);
    }
}