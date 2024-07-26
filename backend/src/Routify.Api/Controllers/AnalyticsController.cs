using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Analytics;
using Routify.Api.Models.Common;
using Routify.Data;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/analytics")]
public class AnalyticsController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet("summary", Name = "GetAppAnalyticsSummary")]
    public async Task<ActionResult<AnalyticsSummaryOutput>> GetAnalyticsSummaryAsync(
        [FromRoute] string appId,
        [FromQuery] string period,
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

        var (from, to) = GetPeriod(period);
        var (previousFrom, previousTo) = GetPreviousPeriod(period, from);
        
        // Current period data
        var currentPeriodData = await databaseContext
            .CompletionLogs
            .Where(log => log.AppId == appId && log.EndedAt >= from && log.EndedAt <= to)
            .GroupBy(log => log.AppId)
            .Select(group => new
            {
                TotalRequests = group.Count(),
                TotalTokens = group.Sum(log => log.InputTokens + log.OutputTokens),
                TotalCost = group.Sum(log => log.InputCost + log.OutputCost),
                AverageDuration = group.Average(log => log.Duration)
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalRequests = 0, TotalTokens = 0, TotalCost = 0m, AverageDuration = 0.0 };

        // Previous period data
        var previousPeriodData = await databaseContext
            .CompletionLogs
            .Where(log => log.AppId == appId && log.EndedAt >= previousFrom && log.EndedAt <= previousTo)
            .GroupBy(log => log.AppId)
            .Select(group => new
            {
                TotalRequests = group.Count(),
                TotalTokens = group.Sum(log => log.InputTokens + log.OutputTokens),
                TotalCost = group.Sum(log => log.InputCost + log.OutputCost),
                AverageDuration = group.Average(log => log.Duration)
            })
            .FirstOrDefaultAsync(cancellationToken) ?? new { TotalRequests = 0, TotalTokens = 0, TotalCost = 0m, AverageDuration = 0.0 };

        var analyticsSummary = new AnalyticsSummaryOutput
        {
            TotalRequests = currentPeriodData.TotalRequests,
            PreviousTotalRequests = previousPeriodData.TotalRequests,
            TotalTokens = currentPeriodData.TotalTokens,
            PreviousTotalTokens = previousPeriodData.TotalTokens,
            TotalCost = currentPeriodData.TotalCost,
            PreviousTotalCost = previousPeriodData.TotalCost,
            AverageDuration = currentPeriodData.AverageDuration,
            PreviousAverageDuration = previousPeriodData.AverageDuration
        };
        
        return Ok(analyticsSummary);
    }

    [HttpGet("histogram", Name = "GetAppAnalyticsHistogram")]
    public async Task<ActionResult<AnalyticsHistogramOutput>> GetAnalyticsHistogramAsync(
        [FromRoute] string appId,
        [FromQuery] string period,
        CancellationToken cancellationToken)
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

        var (from, to) = GetPeriod(period);
        
        var histogramData = await databaseContext
            .CompletionLogs
            .Where(log => log.AppId == appId && log.StartedAt >= from && log.StartedAt <= to)
            .GroupBy(log => new { log.StartedAt.Year, log.StartedAt.Month, log.StartedAt.Day, log.StartedAt.Hour })
            .Select(g => new DateTimeHistogramOutput
            {
                DateTime = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0),
                Count = g.Count()
            })
            .OrderBy(h => h.DateTime)
            .ToDictionaryAsync(x => x.DateTime.ToString("yyyMMddHH"), cancellationToken);

        var requestsHistogram = new List<DateTimeHistogramOutput>();
        for (var date = from; date <= to; date = date.AddHours(1))
        {
            var key = date.ToString("yyyyMMddHH");
            var value = histogramData.GetValueOrDefault(key, new DateTimeHistogramOutput { DateTime = date, Count = 0 });
            requestsHistogram.Add(value);
        }
        
        var output = new AnalyticsHistogramOutput
        {
            Requests = requestsHistogram
        };

        return Ok(output);
    }

    [HttpGet("lists", Name = "GetAppAnalyticsLists")]
    public async Task<ActionResult<AnalyticsHistogramOutput>> GetAnalyticsHistogram(
        [FromRoute] string appId,
        [FromQuery] string period,
        CancellationToken cancellationToken)
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

        var (from, to) = GetPeriod(period);

        var providerAggregation = await databaseContext
            .CompletionLogs
            .Where(log => log.AppId == appId && log.StartedAt >= from && log.StartedAt <= to)
            .GroupBy(log => log.Provider)
            .Select(group => new
            {
                Provider = group.Key,
                TotalRequests = group.Count(),
                TotalTokens = group.Sum(log => log.InputTokens + log.OutputTokens),
                TotalCost = group.Sum(log => log.InputCost + log.OutputCost),
                AverageDuration = group.Average(log => log.Duration)
            })
            .ToListAsync(cancellationToken);
        
        var providerMetrics = providerAggregation
            .Select(x => new MetricsOutput
            {
                Id = x.Provider ?? "Unknown",
                Name = x.Provider ?? "Unknown",
                TotalRequests = x.TotalRequests,
                TotalTokens = x.TotalTokens,
                TotalCost = x.TotalCost,
                AverageDuration = x.AverageDuration
            })
            .ToList();
        
        var modelAggregations = await databaseContext
            .CompletionLogs
            .Where(log => log.AppId == appId && log.StartedAt >= from && log.StartedAt <= to)
            .GroupBy(log => log.Model)
            .Select(group => new
            {
                Model = group.Key,
                TotalRequests = group.Count(),
                TotalTokens = group.Sum(log => log.InputTokens + log.OutputTokens),
                TotalCost = group.Sum(log => log.InputCost + log.OutputCost),
                AverageDuration = group.Average(log => log.Duration)
            })
            .ToListAsync(cancellationToken);
        
        var modelMetrics = modelAggregations
            .Select(x => new MetricsOutput
            {
                Id = x.Model ?? "Unknown",
                Name = x.Model ?? "Unknown",
                TotalRequests = x.TotalRequests,
                TotalTokens = x.TotalTokens,
                TotalCost = x.TotalCost,
                AverageDuration = x.AverageDuration
            })
            .ToList();
        
        var consumerAggregations = await databaseContext
            .CompletionLogs
            .Where(log => log.AppId == appId && log.StartedAt >= from && log.StartedAt <= to)
            .GroupBy(log => log.ConsumerId)
            .Select(group => new
            {
                ConsumerId = group.Key,
                TotalRequests = group.Count(),
                TotalTokens = group.Sum(log => log.InputTokens + log.OutputTokens),
                TotalCost = group.Sum(log => log.InputCost + log.OutputCost),
                AverageDuration = group.Average(log => log.Duration)
            })
            .ToListAsync(cancellationToken);
        
        var consumerIds = consumerAggregations
            .Where(x => !string.IsNullOrEmpty(x.ConsumerId))
            .Select(x => x.ConsumerId!)
            .ToList();

        var consumerMetrics = new List<MetricsOutput>();
        if (consumerIds.Count > 0)
        {
            var consumers = await databaseContext
                .Consumers
                .Where(x => consumerIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id, cancellationToken);

            foreach (var consumerAggregation in consumerAggregations)
            {
                if (string.IsNullOrEmpty(consumerAggregation.ConsumerId))
                    continue;

                if (!consumers.TryGetValue(consumerAggregation.ConsumerId, out var consumer))
                    continue;

                consumerMetrics.Add(new MetricsOutput
                {
                    Id = consumer.Id,
                    Name = consumer.Name,
                    TotalRequests = consumerAggregation.TotalRequests,
                    TotalTokens = consumerAggregation.TotalTokens,
                    TotalCost = consumerAggregation.TotalCost,
                    AverageDuration = consumerAggregation.AverageDuration
                });
            }
        }

        var analyticsLists = new AnalyticsListsOutput
        {
            Providers = providerMetrics,
            Models = modelMetrics,
            Consumers = consumerMetrics
        };
        
        return Ok(analyticsLists);
    }
    
    private static (DateTime, DateTime) GetPeriod(
        string period)
    {
        var now = DateTime.UtcNow;
        var start = period switch
        {
            "today" => now.Date,
            "yesterday" => now.Date.AddDays(-1),
            "7days" => now.Date.AddDays(-7),
            "30days" => now.Date.AddDays(-30),
            _ => throw new ArgumentOutOfRangeException(nameof(period))
        };
        
        return (start, now);
    }
    
    private static (DateTime from, DateTime to) GetPreviousPeriod(
        string period, 
        DateTime from)
    {
        var previousFrom = period switch
        {
            "today" => from.AddDays(-1),
            "yesterday" => from.AddDays(-1),
            "7days" => from.AddDays(-7),
            "30days" => from.AddDays(-30),
            _ => throw new ArgumentOutOfRangeException(nameof(period))
        };

        var previousTo = from;
        return (previousFrom, previousTo);
    }
}