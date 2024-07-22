using Routify.Core.Utils;
using Routify.Gateway.Utils;
using StackExchange.Redis;

namespace Routify.Gateway.Services;

internal class CacheService(IDatabase redisDatabase)
{
    public async Task<T?> GetAsync<T>(
        string key) where T : class
    {
        var redisKey = RedisUtils.BuildKey(key);
        string? value = await redisDatabase.StringGetAsync(redisKey);
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
        var redisKey = RedisUtils.BuildKey(key);
        var value = RoutifyJsonSerializer.Serialize(data);
        await redisDatabase.StringSetAsync(redisKey, value, expiry);
    }
}