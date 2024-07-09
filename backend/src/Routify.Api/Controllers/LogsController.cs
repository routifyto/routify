using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Common;
using Routify.Api.Models.LogModels;
using Routify.Data;

namespace Routify.Api.Controllers;

[Route("/v1/apps/{appId}/logs")]
public class LogsController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet("text", Name = "GetTextLogs")]
    public async Task<ActionResult<PaginatedPayload<TextLogRowPayload>>> GetTextLogsAsync(
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
            .TextLogs
            .Where(x => x.AppId == appId);

        if (!string.IsNullOrWhiteSpace(after))
            query = query.Where(x => x.Id.CompareTo(after) < 0);

        // Limit the number of items to fetch
        limit = Math.Max(1, Math.Min(limit, 100));
        var textLogs = await query
            .OrderByDescending(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var textLogPayloads = textLogs
            .Select(log => new TextLogRowPayload
            {
                Id = log.Id,
                RouteId = log.RouteId,
                Path = log.Path,
                Provider = log.Provider,
                Model = log.Model,
                InputTokens = log.InputTokens,
                OutputTokens = log.OutputTokens,
                InputCost = log.InputCost,
                OutputCost = log.OutputCost,
                EndedAt = log.EndedAt,
                Duration = log.Duration
            })
            .ToList();

        var hasNext = textLogs.Count == limit;
        var nextCursor = hasNext ? textLogs.Last().Id : null;

        return new PaginatedPayload<TextLogRowPayload>
        {
            Items = textLogPayloads,
            HasNext = hasNext,
            NextCursor = nextCursor
        };
    }
    
    [HttpGet("text/{textLogId}", Name = "GetTextLog")]
    public async Task<ActionResult<TextLogPayload>> GetTextLogAsync(
        [FromRoute] string appId,
        [FromRoute] string textLogId,
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

        var textLog = await databaseContext
            .TextLogs
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == textLogId, cancellationToken);

        if (textLog is null)
            return NotFound();
        
        var route = await databaseContext
            .Routes
            .SingleOrDefaultAsync(x => x.Id == textLog.RouteId, cancellationToken);
        
        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == textLog.AppProviderId, cancellationToken);

        return new TextLogPayload
        {
            Id = textLog.Id,
            RouteId = textLog.RouteId,
            Path = textLog.Path,
            Provider = textLog.Provider,
            Model = textLog.Model,
            AppProviderId = textLog.AppProviderId,
            RouteProviderId = textLog.RouteProviderId,
            ApiKeyId = textLog.ApiKeyId,
            SessionId = textLog.SessionId,
            RequestBody = textLog.RequestBody,
            ResponseStatusCode = textLog.ResponseStatusCode,
            ResponseBody = textLog.ResponseBody,
            InputTokens = textLog.InputTokens,
            OutputTokens = textLog.OutputTokens,
            InputCost = textLog.InputCost,
            OutputCost = textLog.OutputCost,
            StartedAt = textLog.StartedAt,
            EndedAt = textLog.EndedAt,
            Duration = textLog.Duration,
            Route = route is null ? null : new LogRoutePayload
            {
                Id = route.Id,
                Name = route.Name,
                Description = route.Description,
                Path = route.Path
            },
            AppProvider = appProvider is null ? null : new LogAppProviderPayload
            {
                Id = appProvider.Id,
                Name = appProvider.Name,
                Description = appProvider.Description,
                Alias = appProvider.Alias
            }
        };
    }
}