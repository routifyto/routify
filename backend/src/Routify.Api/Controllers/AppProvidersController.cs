using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.AppProviders;
using Routify.Api.Models.Common;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/providers")]
public class AppProvidersController(
    DatabaseContext databaseContext) 
    : BaseController
{
    [HttpGet(Name = "GetAppProviders")]
    public async Task<ActionResult<PaginatedPayload<AppProviderPayload>>> GetProvidersAsync(
        [FromRoute] string appId,
        [FromQuery] string? after,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var query = databaseContext
            .AppProviders
            .Where(x => x.AppId == appId);

        if (!string.IsNullOrWhiteSpace(after))
            query = query.Where(x => x.Id.CompareTo(after) > 0);

        // Limit the number of items to fetch
        limit = Math.Max(1, Math.Min(limit, 100));
        var appProviders = await query
            .OrderBy(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var appProviderPayloads = appProviders
            .Select(MapToPayload)
            .ToList();
        
        var hasNext = appProviders.Count == limit;
        var nextCursor = hasNext ? appProviders.Last().Id : null;
        var payload = new PaginatedPayload<AppProviderPayload>
        {
            Items = appProviderPayloads,
            HasNext = hasNext,
            NextCursor = nextCursor
        };
        
        return Ok(payload);
    }
    
    [HttpGet("{appProviderId}", Name = "GetAppProvider")]
    public async Task<ActionResult<AppProviderPayload>> GetProviderAsync(
        [FromRoute] string appId,
        [FromRoute] string appProviderId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == appProviderId, cancellationToken);

        if (appProvider is null)
            return NotFound();

        var payload = MapToPayload(appProvider);
        return Ok(payload);
    }
    
    [HttpPost(Name = "CreateAppProvider")]
    public async Task<ActionResult<AppProviderPayload>> CreateProviderAsync(
        [FromRoute] string appId,
        [FromBody] AppProviderInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null || currentAppUser.Role == AppUserRole.Member)
            return Forbid();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var appProvider = new AppProvider
        {
            Id = RoutifyId.Generate(IdType.AppProvider),
            AppId = appId,
            Provider = input.Provider,
            Name = input.Name,
            Alias = input.Alias,
            Description = input.Description,
            Attrs = input.Attrs,
            CreatedBy = CurrentUserId,
            CreatedAt = DateTime.UtcNow,
            VersionId = app.VersionId,
            Status = AppProviderStatus.Active
        };

        databaseContext.AppProviders.Add(appProvider);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var payload = MapToPayload(appProvider);
        return Ok(payload);
    }
    
    [HttpPut("{appProviderId}", Name = "UpdateAppProvider")]
    public async Task<ActionResult<AppProviderPayload>> UpdateProviderAsync(
        [FromRoute] string appId,
        [FromRoute] string appProviderId,
        [FromBody] AppProviderInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null || currentAppUser.Role == AppUserRole.Member)
            return Forbid();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == appProviderId, cancellationToken);

        if (appProvider is null)
            return NotFound();

        appProvider.Provider = input.Provider;
        appProvider.Name = input.Name;
        appProvider.Alias = input.Alias;
        appProvider.Description = input.Description;
        appProvider.Attrs = input.Attrs;
        
        await databaseContext.SaveChangesAsync(cancellationToken);

        var payload = MapToPayload(appProvider);
        return Ok(payload);
    }
    
    [HttpDelete("{appProviderId}", Name = "DeleteAppProvider")]
    public async Task<ActionResult<DeletePayload>> DeleteProviderAsync(
        [FromRoute] string appId,
        [FromRoute] string appProviderId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null || currentAppUser.Role == AppUserRole.Member)
            return Forbid();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == appProviderId, cancellationToken);

        if (appProvider is null)
            return NotFound();

        databaseContext.Remove(appProvider);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var payload = new DeletePayload
        {
            Id = appProvider.Id
        };
        
        return Ok(payload);
    }
    
    private static AppProviderPayload MapToPayload(
        AppProvider appProvider)
    {
        return new AppProviderPayload
        {
            Id = appProvider.Id,
            Name = appProvider.Name,
            Alias = appProvider.Alias,
            Description = appProvider.Description,
            Provider = appProvider.Provider,
            Attrs = appProvider.Attrs
        };
    }
}