using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Apps;
using Routify.Api.Models.Common;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps")]
public class AppsController(
    DatabaseContext databaseContext) : BaseController
{
    [HttpGet("{appId}", Name = "GetApp")]
    public async Task<ActionResult<AppPayload>> GetAppAsync(
        [FromRoute] string appId,
        CancellationToken cancellationToken = default)
    {
        var app = await databaseContext
            .Apps
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == appId, cancellationToken);
        
        if (app is null)
            return NotFound();

        var payload = new AppPayload
        {
            Id = app.Id,
            Name = app.Name
        };
        
        return Ok(payload);
    }
    
    [HttpPost(Name = "CreateApp")]
    public async Task<ActionResult<AppPayload>> CreateAppAsync(
        [FromBody] AppInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();
        
        var app = new App
        {
            Id = RoutifyId.Generate(IdType.App),
            Name = input.Name,
            Description = input.Description,
            Status = AppStatus.Active,
            CreatedBy = CurrentUserId,
            CreatedAt = DateTime.UtcNow,
            VersionId = RoutifyId.Generate(IdType.Version)
        };

        var appUser = new AppUser
        {
            Id = RoutifyId.Generate(IdType.AppUser),
            AppId = app.Id,
            UserId = CurrentUserId,
            Role = AppUserRole.Owner,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = CurrentUserId
        };
        
        await databaseContext.Apps.AddAsync(app, cancellationToken);
        await databaseContext.AppUsers.AddAsync(appUser, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        var payload = new AppPayload
        {
            Id = app.Id,
            Name = app.Name
        };
        
        return Ok(payload);
    }
    
    [HttpPut(Name = "UpdateApp")]
    public async Task<ActionResult<AppPayload>> UpdateAppAsync(
        [FromRoute] string appId,
        [FromBody] AppInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();
        
        var appUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (appUser is null || appUser.Role != AppUserRole.Owner)
            return Forbid();
        
        var app = appUser.App;
        if (app is null)
            return NotFound();
        
        app.Name = input.Name;
        app.Description = input.Description;
        app.UpdatedBy = CurrentUserId;
        app.UpdatedAt = DateTime.UtcNow;
        app.VersionId = RoutifyId.Generate(IdType.Version);
        
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        return Ok(new AppPayload
        {
            Id = app.Id,
            Name = app.Name
        });
    }
    
    [HttpDelete("{appId}", Name = "DeleteApp")]
    public async Task<ActionResult<DeletePayload>> DeleteAppAsync(
        [FromRoute] string appId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();
        
        var appUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (appUser is null || appUser.Role != AppUserRole.Owner)
            return Forbid();
        
        var app = appUser.App;
        if (app is null)
            return NotFound();
        
        databaseContext.Apps.Remove(app);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var payload = new DeletePayload
        {
            Id = app.Id
        };
        
        return Ok(payload);
    }
}
