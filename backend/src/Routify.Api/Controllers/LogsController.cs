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
    [HttpGet("completions", Name = "GetCompletionLogs")]
    public async Task<ActionResult<PaginatedPayload<CompletionLogRowPayload>>> GetCompletionLogsAsync(
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
            .CompletionLogs
            .Where(x => x.AppId == appId);

        if (!string.IsNullOrWhiteSpace(after))
            query = query.Where(x => x.Id.CompareTo(after) < 0);

        // Limit the number of items to fetch
        limit = Math.Max(1, Math.Min(limit, 100));
        var completionLogs = await query
            .OrderByDescending(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var completionLogPayloads = completionLogs
            .Select(log => new CompletionLogRowPayload
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

        var hasNext = completionLogs.Count == limit;
        var nextCursor = hasNext ? completionLogs.Last().Id : null;

        return new PaginatedPayload<CompletionLogRowPayload>
        {
            Items = completionLogPayloads,
            HasNext = hasNext,
            NextCursor = nextCursor
        };
    }
    
    [HttpGet("completions/{completionLogId}", Name = "GetCompletionLog")]
    public async Task<ActionResult<CompletionLogPayload>> GetCompletionLogAsync(
        [FromRoute] string appId,
        [FromRoute] string completionLogId,
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

        var completionLog = await databaseContext
            .CompletionLogs
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == completionLogId, cancellationToken);

        if (completionLog is null)
            return NotFound();
        
        var route = await databaseContext
            .Routes
            .SingleOrDefaultAsync(x => x.Id == completionLog.RouteId, cancellationToken);
        
        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == completionLog.AppProviderId, cancellationToken);

        return new CompletionLogPayload
        {
            Id = completionLog.Id,
            RouteId = completionLog.RouteId,
            Path = completionLog.Path,
            Provider = completionLog.Provider,
            Model = completionLog.Model,
            AppProviderId = completionLog.AppProviderId,
            RouteProviderId = completionLog.RouteProviderId,
            ApiKeyId = completionLog.ApiKeyId,
            SessionId = completionLog.SessionId,
            RequestBody = completionLog.RequestBody,
            ResponseStatusCode = completionLog.ResponseStatusCode,
            ResponseBody = completionLog.ResponseBody,
            InputTokens = completionLog.InputTokens,
            OutputTokens = completionLog.OutputTokens,
            InputCost = completionLog.InputCost,
            OutputCost = completionLog.OutputCost,
            StartedAt = completionLog.StartedAt,
            EndedAt = completionLog.EndedAt,
            Duration = completionLog.Duration,
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