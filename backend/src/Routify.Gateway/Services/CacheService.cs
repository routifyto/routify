using Routify.Core.Utils;
using StackExchange.Redis;

namespace Routify.Gateway.Services;

internal class CacheService(IDatabase redisDatabase)
{
    public async Task<T?> GetAsync<T>(
        string key) where T : class
    {
        string? value = await redisDatabase.StringGetAsync(key);
        if (string.IsNullOrEmpty(value))
            return null;
        
        var data = RoutifyJsonSerializer.Deserialize<T>(value);
        return data;
    }
    
    public async Task SetAsync<T>(
        string key,
        T data,
        TimeSpan? expiry = null)
    {
        var value = RoutifyJsonSerializer.Serialize(data);
        await redisDatabase.StringSetAsync(key, value, expiry);
    }
}