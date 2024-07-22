namespace Routify.Gateway.Utils;

internal static class RedisUtils
{
    public static string BuildKey(
        string key)
    {
        return $"routify:{key}";
    }
}