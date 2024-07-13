using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.AppProviders;
using Routify.Api.Models.Common;
using Routify.Core.Services;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Enums;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/providers")]
public class AppProvidersController(
    DatabaseContext databaseContext,
    EncryptionService encryptionService) 
    : BaseController
{
    [HttpGet(Name = "GetAppProviders")]
    public async Task<ActionResult<PaginatedOutput<AppProviderOutput>>> GetProvidersAsync(
        [FromRoute] string appId,
        [FromQuery] string? after,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
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

        var appProviderOutputs = appProviders
            .Select(MapToOutput)
            .ToList();
        
        var hasNext = appProviders.Count == limit;
        var nextCursor = hasNext ? appProviders.Last().Id : null;
        var output = new PaginatedOutput<AppProviderOutput>
        {
            Items = appProviderOutputs,
            HasNext = hasNext,
            NextCursor = nextCursor
        };
        
        return Ok(output);
    }
    
    [HttpGet("{appProviderId}", Name = "GetAppProvider")]
    public async Task<ActionResult<AppProviderOutput>> GetProviderAsync(
        [FromRoute] string appId,
        [FromRoute] string appProviderId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == appProviderId, cancellationToken);

        if (appProvider is null)
            return NotFound();

        var output = MapToOutput(appProvider);
        return Ok(output);
    }
    
    [HttpPost(Name = "CreateAppProvider")]
    public async Task<ActionResult<AppProviderOutput>> CreateProviderAsync(
        [FromRoute] string appId,
        [FromBody] AppProviderInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null || currentAppUser.Role == AppRole.Member)
            return Forbid();

        var appProvider = new AppProvider
        {
            Id = RoutifyId.Generate(IdType.AppProvider),
            AppId = appId,
            Provider = input.Provider,
            Name = input.Name,
            Alias = input.Alias,
            Description = input.Description,
            Attrs = EncryptAttrs(input.Attrs),
            CreatedBy = CurrentUserId,
            CreatedAt = DateTime.UtcNow,
            VersionId = RoutifyId.Generate(IdType.Version),
            Status = AppProviderStatus.Active
        };

        databaseContext.AppProviders.Add(appProvider);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = MapToOutput(appProvider);
        return Ok(output);
    }
    
    [HttpPut("{appProviderId}", Name = "UpdateAppProvider")]
    public async Task<ActionResult<AppProviderOutput>> UpdateProviderAsync(
        [FromRoute] string appId,
        [FromRoute] string appProviderId,
        [FromBody] AppProviderInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null || currentAppUser.Role == AppRole.Member)
            return Forbid();

        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == appProviderId, cancellationToken);

        if (appProvider is null)
            return NotFound();

        appProvider.Provider = input.Provider;
        appProvider.Name = input.Name;
        appProvider.Alias = input.Alias;
        appProvider.Description = input.Description;
        appProvider.Attrs = EncryptAttrs(input.Attrs);
        
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = MapToOutput(appProvider);
        return Ok(output);
    }
    
    [HttpDelete("{appProviderId}", Name = "DeleteAppProvider")]
    public async Task<ActionResult<DeleteOutput>> DeleteProviderAsync(
        [FromRoute] string appId,
        [FromRoute] string appProviderId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null || currentAppUser.Role == AppRole.Member)
            return Forbid();

        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == appProviderId, cancellationToken);

        if (appProvider is null)
            return NotFound();

        databaseContext.Remove(appProvider);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = new DeleteOutput
        {
            Id = appProvider.Id
        };
        
        return Ok(output);
    }
    
    private AppProviderOutput MapToOutput(
        AppProvider appProvider)
    {
        return new AppProviderOutput
        {
            Id = appProvider.Id,
            Name = appProvider.Name,
            Alias = appProvider.Alias,
            Description = appProvider.Description,
            Provider = appProvider.Provider,
            Attrs = DecryptAttrs(appProvider.Attrs)
        };
    }

    private Dictionary<string, string> EncryptAttrs(
        Dictionary<string, string> attrs)
    {
        var encryptedAttrs = new Dictionary<string, string>();
        foreach (var (key, value) in attrs)
        {
            var encryptedValue = encryptionService.Encrypt(value);
            encryptedAttrs[key] = encryptedValue;
        }
        return encryptedAttrs;
    }
    
    private Dictionary<string, string> DecryptAttrs(
        Dictionary<string, string> attrs)
    {
        var decryptedAttrs = new Dictionary<string, string>();
        foreach (var (key, value) in attrs)
        {
            var decryptedValue = encryptionService.Decrypt(value);
            decryptedAttrs[key] = decryptedValue;
        }
        return decryptedAttrs;
    }
}