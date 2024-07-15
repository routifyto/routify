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

        return new CompletionLogOutput
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
            GatewayRequest = new RequestLogOutput
                {
                    Url = completionLog.GatewayRequest.Url,
                    Method = completionLog.GatewayRequest.Method,
                    Headers = completionLog.GatewayRequest.Headers,
                    Body = completionLog.GatewayRequest.Body
                },
            ProviderRequest = completionLog.ProviderRequest != null ?
                new RequestLogOutput
                {
                    Url = completionLog.ProviderRequest.Url,
                    Method = completionLog.ProviderRequest.Method,
                    Headers = completionLog.ProviderRequest.Headers,
                    Body = completionLog.ProviderRequest.Body
                } : null,
            GatewayResponse = completionLog.GatewayResponse != null ?
                new ResponseLogOutput
                {
                    StatusCode = completionLog.GatewayResponse.StatusCode,
                    Headers = completionLog.GatewayResponse.Headers,
                    Body = completionLog.GatewayResponse.Body
                } : null,
            ProviderResponse = completionLog.ProviderResponse != null ?
                new ResponseLogOutput
                {
                    StatusCode = completionLog.ProviderResponse.StatusCode,
                    Headers = completionLog.ProviderResponse.Headers,
                    Body = completionLog.ProviderResponse.Body
                } : null,
            InputTokens = completionLog.InputTokens,
            OutputTokens = completionLog.OutputTokens,
            InputCost = completionLog.InputCost,
            OutputCost = completionLog.OutputCost,
            StartedAt = completionLog.StartedAt,
            EndedAt = completionLog.EndedAt,
            Duration = completionLog.Duration,
            Route = route is null ? null : new LogRouteOutput
            {
                Id = route.Id,
                Name = route.Name,
                Description = route.Description,
                Path = route.Path
            },
            AppProvider = appProvider is null ? null : new LogAppProviderOutput
            {
                Id = appProvider.Id,
                Name = appProvider.Name,
                Description = appProvider.Description,
                Alias = appProvider.Alias
            }
        };
    }
}