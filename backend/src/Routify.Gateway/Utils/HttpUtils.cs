namespace Routify.Gateway.Utils;

internal static class HttpUtils
{
    private static readonly HashSet<int> _statusCodesForRetry =
    [
        408, // RequestTimeout
        429, // TooManyRequests
        500, // InternalServerError
        502, // BadGateway
        503, // ServiceUnavailable
        504 // GatewayTimeout
    ];
    
    public static bool ShouldRetry(int statusCode) => _statusCodesForRetry.Contains(statusCode);
}