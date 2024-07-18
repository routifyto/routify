using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Common;
using Routify.Api.Models.LogModels;
using Routify.Data;
using Routify.Data.Models;
using Route = Routify.Data.Models.Route;

namespace Routify.Api.Controllers;

[Route("/v1/apps/{appId}/logs")]
public class LogsController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet("completions", Name = "GetCompletionLogs")]
    public async Task<ActionResult<PaginatedOutput<CompletionLogRowOutput>>> GetCompletionLogsAsync(
        [FromRoute] string appId,
        [FromQuery] string? after,
        [FromQuery] int limit = 20,
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

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }

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

        var completionLogOutputs = completionLogs
            .Select(log => new CompletionLogRowOutput
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

        return new PaginatedOutput<CompletionLogRowOutput>
        {
            Items = completionLogOutputs,
            HasNext = hasNext,
            NextCursor = nextCursor
        };
    }
    
    [HttpGet("completions/{completionLogId}", Name = "GetCompletionLog")]
    public async Task<ActionResult<CompletionLogOutput>> GetCompletionLogAsync(
        [FromRoute] string appId,
        [FromRoute] string completionLogId,
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

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }

        var completionLog = await databaseContext
            .CompletionLogs
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == completionLogId, cancellationToken);

        if (completionLog is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.CompletionLogNotFound,
                Message = "Completion log not found"
            });
        }
        
        var route = await databaseContext
            .Routes
            .SingleOrDefaultAsync(x => x.Id == completionLog.RouteId, cancellationToken);
        
        var appProvider = await databaseContext
            .AppProviders
            .SingleOrDefaultAsync(x => x.Id == completionLog.AppProviderId, cancellationToken);

        var output = new CompletionLogOutput
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
            ConsumerId = completionLog.ConsumerId,
            OutgoingRequestsCount = completionLog.OutgoingRequestsCount,
            RequestUrl = completionLog.RequestUrl,
            RequestMethod = completionLog.RequestMethod,
            RequestBody = completionLog.RequestBody,
            RequestHeaders = completionLog.RequestHeaders,
            StatusCode = completionLog.StatusCode,
            ResponseBody = completionLog.ResponseBody,
            ResponseHeaders = completionLog.ResponseHeaders,
            InputTokens = completionLog.InputTokens,
            OutputTokens = completionLog.OutputTokens,
            InputCost = completionLog.InputCost,
            OutputCost = completionLog.OutputCost,
            StartedAt = completionLog.StartedAt,
            EndedAt = completionLog.EndedAt,
            Duration = completionLog.Duration,
            Route = MapRoute(route),
            AppProvider = MapAppProvider(appProvider)
        };
        
        return Ok(output);
    }

    [HttpGet("completions/{completionLogId}/outgoing", Name = "GetCompletionOutgoingLogs")]
    public async Task<ActionResult<List<CompletionOutgoingLogOutput>>>
        GetCompletionOutgoingLogsAsync(
            [FromRoute] string appId,
            [FromRoute] string completionLogId,
            [FromQuery] string? after,
            [FromQuery] int limit = 20,
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

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }

        
        var completionOutgoingLogs = await databaseContext
            .CompletionOutgoingLogs
            .Where(x => x.AppId == appId && x.IncomingLogId == completionLogId)
            .ToListAsync(cancellationToken);
        
        var appProviderIds = completionOutgoingLogs
            .Select(log => log.AppProviderId)
            .Distinct()
            .ToList();
        
        var appProviders = await databaseContext
            .AppProviders
            .Where(x => appProviderIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);
        
        var completionOutgoingLogOutputs = completionOutgoingLogs
            .Select(log => new CompletionOutgoingLogOutput
            {
                Id = log.Id,
                IncomingLogId = log.IncomingLogId,
                Provider = log.Provider,
                AppProviderId = log.AppProviderId,
                RouteProviderId = log.RouteProviderId,
                RequestUrl = log.RequestUrl,
                RequestMethod = log.RequestMethod,
                RequestHeaders = log.RequestHeaders,
                RequestBody = log.RequestBody,
                StatusCode = log.StatusCode,
                ResponseBody = log.ResponseBody,
                ResponseHeaders = log.ResponseHeaders,
                StartedAt = log.StartedAt,
                EndedAt = log.EndedAt,
                Duration = log.Duration,
                
                AppProvider = MapAppProvider(appProviders.GetValueOrDefault(log.AppProviderId))
            })
            .ToList();
        
        return Ok(completionOutgoingLogOutputs);
    }
    
    private static LogRouteOutput? MapRoute(
        Route? route)
    {
        if (route is null)
            return null;

        return new LogRouteOutput
        {
            Id = route.Id,
            Name = route.Name,
            Description = route.Description,
            Path = route.Path
        };
    }
    
    private static LogAppProviderOutput? MapAppProvider(
        AppProvider? appProvider)
    {
        if (appProvider is null)
            return null;

        return new LogAppProviderOutput
        {
            Id = appProvider.Id,
            Name = appProvider.Name,
            Description = appProvider.Description,
            Alias = appProvider.Alias
        };
    }
}