using Routify.Data.Common;
using Routify.Gateway.Utils;
using StackExchange.Redis;

namespace Routify.Gateway.Services;

internal class CostService(
    IDatabase database)
{
    public async Task SaveCost(
        string id,
        decimal cost)
    {
        var doubleValue = (double) cost;
        
        await database.StringIncrementAsync(GetTodayKey(id), doubleValue);
        await database.StringIncrementAsync(GetMonthKey(id), doubleValue);
    }
    
    public async Task<bool> HasReachedCostLimit(
        string id,
        CostLimitConfig config)
    {
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
    
    private static string GetTodayKey(string id) => RedisUtils.BuildKey($"cost:{id}:{DateTime.UtcNow:yyyyMMdd}");
    private static string GetMonthKey(string id) => RedisUtils.BuildKey($"cost:{id}:{DateTime.UtcNow:yyyyMM}");
}