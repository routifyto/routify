using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Analytics;
using Routify.Data;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/analytics")]
public class AnalyticsController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet("summary", Name = "GetAppAnalyticsSummary")]
    public async Task<ActionResult<AnalyticsSummaryPayload>> GetAnalyticsSummaryAsync(
        [FromRoute] string appId,
        [FromQuery] string period,
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

        var analyticsSummary = new AnalyticsSummaryPayload
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
    public async Task<ActionResult<AnalyticsHistogramPayload>> GetAnalyticsHistogramAsync(
        [FromRoute] string appId,
        [FromQuery] string period,
        CancellationToken cancellationToken)
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
        
        var (from, to) = GetPeriod(period);
        
        var histogramData = await databaseContext
            .CompletionLogs
            .Where(log => log.AppId == appId && log.StartedAt >= from && log.StartedAt <= to)
            .GroupBy(log => new { log.StartedAt.Year, log.StartedAt.Month, log.StartedAt.Day, log.StartedAt.Hour })
            .Select(g => new DateTimeHistogramPayload
            {
                DateTime = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, 0, 0),
                Count = g.Count()
            })
            .OrderBy(h => h.DateTime)
            .ToDictionaryAsync(x => x.DateTime.ToString("yyyMMddHH"), cancellationToken);

        var requestsHistogram = new List<DateTimeHistogramPayload>();
        for (var date = from; date <= to; date = date.AddHours(1))
        {
            var key = date.ToString("yyyyMMddHH");
            var value = histogramData.GetValueOrDefault(key, new DateTimeHistogramPayload { DateTime = date, Count = 0 });
            requestsHistogram.Add(value);
        }
        
        var payload = new AnalyticsHistogramPayload
        {
            Requests = requestsHistogram
        };

        return Ok(payload);
    }

    [HttpGet("lists", Name = "GetAppAnalyticsLists")]
    public async Task<ActionResult<AnalyticsHistogramPayload>> GetAnalyticsHistogram(
        [FromRoute] string appId,
        [FromQuery] string period,
        CancellationToken cancellationToken)
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

        var analyticsLists = new AnalyticsListsPayload
        {
            Providers = providerAggregation
                .Select(x => new MetricsPayload
                {
                    Id = x.Provider ?? "Unknown",
                    TotalRequests = x.TotalRequests,
                    TotalTokens = x.TotalTokens,
                    TotalCost = x.TotalCost,
                    AverageDuration = x.AverageDuration
                })
                .ToList(),
            Models = modelAggregations
                .Select(x => new MetricsPayload
                {
                    Id = x.Model ?? "Unknown",
                    TotalRequests = x.TotalRequests,
                    TotalTokens = x.TotalTokens,
                    TotalCost = x.TotalCost,
                    AverageDuration = x.AverageDuration
                })
                .ToList()
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