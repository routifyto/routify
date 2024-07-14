using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Common;
using Routify.Api.Models.Workspaces;
using Routify.Data;

namespace Routify.Api.Controllers;

[Route("v1/workspace")]
public class WorkspaceController(
    DatabaseContext databaseContext) 
    : BaseController
{
    [HttpGet(Name = "GetWorkspace")]
    public async Task<ActionResult<WorkspaceOutput>> GetWorkspaceAsync(
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
        {
            return Unauthorized(new ApiErrorOutput
            {
                Code = ApiError.Unauthorized,
                Message = "Unauthorized access"
            });
        }

        var user = await databaseContext
            .Users
            .FindAsync(CurrentUserId, cancellationToken);

        if (user is null)
        {
            return Unauthorized(new ApiErrorOutput
            {
                Code = ApiError.Unauthorized,
                Message = "Unauthorized access"
            });
        }

        var userApps = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .Where(x => x.UserId == user.Id)
            .ToListAsync(cancellationToken);
        
        var output = new WorkspaceOutput
        {
            User = new WorkspaceUserOutput
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

            var appOutput = new WorkspaceAppOutput
            {
                Id = app.Id,
                Name = app.Name,
                Role = userApp.Role,
                Description = app.Description
            };
            
            output.Apps.Add(appOutput);
        }
        
        return Ok(output);
    }
}