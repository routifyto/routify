using System.Text;

namespace Routify.Gateway.Extensions;

internal static class HttpRequestExtensions
{
    public static async Task<string?> ReadBodyAsync(
        this HttpRequest request,
        CancellationToken cancellationToken)
    {
        if (request.Body.CanRead == false)
            return null;

        using var reader = new StreamReader(request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync(cancellationToken);
    }
    
    public static string? GetAppId(
        this HttpRequest request)
    {
        return request.Headers.TryGetValue("routify-app", out var values) ? values.FirstOrDefault() : null;
    }
    
    public static string? GetConsumer(
        this HttpRequest request)
    {
        return request.Headers.TryGetValue("routify-consumer", out var values) ? values.FirstOrDefault() : null;
    }
}