using Routify.Data.Common;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Utils;
using StackExchange.Redis;

namespace Routify.Gateway.Services;

internal class CostService(
    IDatabase database)
{
    public async Task<bool> HasReachedCostLimit(
        RequestContext context)
    {
        if (await HasReachedCostLimit(context.Route.Id, context.Route.CostLimitConfig))
            return true;
        
        if (context.Consumer != null && await HasReachedCostLimit(context.Consumer.Id, context.Consumer.CostLimitConfig))
            return true;
        
        return false;
    }
    
    public async Task SaveCost(
        RequestContext context,
        decimal cost)
    {
        await SaveCost(context.Route.Id, context.Route.CostLimitConfig, cost);
        
        if (context.Consumer != null)
            await SaveCost(context.Consumer.Id, context.Consumer.CostLimitConfig, cost);
    }
    
    private async Task<bool> HasReachedCostLimit(
        string id,
        CostLimitConfig? config)
    {
        if (config == null)
            return false;
        
        if (!config.Enabled)
            return false;

        if (config.DailyLimit.HasValue)
        {
            var todayKey = GetTodayKey(id);
            string? todayValue = await database.StringGetAsync(todayKey);
            if (string.IsNullOrWhiteSpace(todayValue))
                return false;
            
            if (!decimal.TryParse(todayValue, out var todayCost))
                return false;
            
            if (todayCost > config.DailyLimit.Value)
                return true;
        }
        
        if (config.MonthlyLimit.HasValue)
        {
            var monthKey = GetMonthKey(id);
            string? monthValue = await database.StringGetAsync(monthKey);
            if (string.IsNullOrWhiteSpace(monthValue))
                return false;
            
            if (!decimal.TryParse(monthValue, out var monthCost))
                return false;
            
            if (monthCost > config.MonthlyLimit.Value)
                return true;
        }
        
        return false;
    }
    
    private async Task SaveCost(
        string id,
        CostLimitConfig? config,
        decimal cost)
    {
        if (config == null)
            return;
        
        if (!config.Enabled)
            return;
        
        var doubleValue = (double) cost;
        
        await database.StringIncrementAsync(GetTodayKey(id), doubleValue);
        await database.StringIncrementAsync(GetMonthKey(id), doubleValue);
    }

    
    private static string GetTodayKey(string id) => RedisUtils.BuildKey($"cost:{id}:{DateTime.UtcNow:yyyyMMdd}");
    private static string GetMonthKey(string id) => RedisUtils.BuildKey($"cost:{id}:{DateTime.UtcNow:yyyyMM}");
}