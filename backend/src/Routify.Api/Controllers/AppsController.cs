using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Apps;
using Routify.Api.Models.Common;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Enums;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps")]
public class AppsController(
    DatabaseContext databaseContext) : BaseController
{
    [HttpGet("{appId}", Name = "GetApp")]
    public async Task<ActionResult<AppOutput>> GetAppAsync(
        [FromRoute] string appId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();
        
        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (currentAppUser is null)
            return Forbid();
        
        var app = currentAppUser.App;
        if (app is null)
            return NotFound();
        
        var output = new AppOutput
        {
            Id = app.Id,
            Name = app.Name,
            Role = currentAppUser.Role
        };
        
        return Ok(output);
    }
    
    [HttpPost(Name = "CreateApp")]
    public async Task<ActionResult<AppOutput>> CreateAppAsync(
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
            Role = AppRole.Owner,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = CurrentUserId
        };
        
        await databaseContext.Apps.AddAsync(app, cancellationToken);
        await databaseContext.AppUsers.AddAsync(appUser, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        var output = new AppOutput
        {
            Id = app.Id,
            Name = app.Name,
            Description = app.Description,
            Role = appUser.Role
        };
        
        return Ok(output);
    }
    
    [HttpPut("{appId}", Name = "UpdateApp")]
    public async Task<ActionResult<AppOutput>> UpdateAppAsync(
        [FromRoute] string appId,
        [FromBody] AppInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();
        
        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (currentAppUser is null || currentAppUser.Role != AppRole.Owner)
            return Forbid();
        
        var app = currentAppUser.App;
        if (app is null)
            return NotFound();
        
        app.Name = input.Name;
        app.Description = input.Description;
        app.UpdatedBy = CurrentUserId;
        app.UpdatedAt = DateTime.UtcNow;
        app.VersionId = RoutifyId.Generate(IdType.Version);
        
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        return Ok(new AppOutput
        {
            Id = app.Id,
            Name = app.Name,
            Description = app.Description,
            Role = currentAppUser.Role
        });
    }
    
    [HttpDelete("{appId}", Name = "DeleteApp")]
    public async Task<ActionResult<DeleteOutput>> DeleteAppAsync(
        [FromRoute] string appId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();
        
        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (currentAppUser is null || currentAppUser.Role != AppRole.Owner)
            return Forbid();
        
        var app = currentAppUser.App;
        if (app is null)
            return NotFound();
        
        databaseContext.Apps.Remove(app);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = new DeleteOutput
        {
            Id = app.Id
        };
        
        return Ok(output);
    }
}
