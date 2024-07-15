using Routify.Data.Common;

namespace Routify.Gateway.Extensions;

internal static class HttpRequestExtensions
{
    public static string? GetConsumer(
        this HttpRequest request)
    {
        return request.Headers.TryGetValue("routify-consumer", out var values) ? values.FirstOrDefault() : null;
    }

    public static RequestLog ToRequestLog(
        this HttpRequest request)
    {
        var fullUrl = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
        // var headers = request
        //     .Headers
        //     .ToDictionary(x => x.Key, x => x.Value.ToString());
        
        return new RequestLog
        {
            Url = fullUrl,
            Method = request.Method,
            Headers = new Dictionary<string, string>(),
        };
    }

    public static RequestLog ToRequestLog(
        this HttpRequestMessage requestMessage,
        string? body)
    {
        var fullUrl = requestMessage.RequestUri?.ToString();
        // var headers = requestMessage
        //     .Headers
        //     .ToDictionary(x => x.Key, x => string.Join(",", x.Value));

        return new RequestLog
        {
            Url = fullUrl,
            Method = requestMessage.Method.ToString(),
            Headers = new Dictionary<string, string>(),
            Body = body
        };
    }
}