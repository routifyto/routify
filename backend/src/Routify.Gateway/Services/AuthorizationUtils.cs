using Routify.Core.Extensions;
using Routify.Gateway.Models.Data;

namespace Routify.Gateway.Services;

internal class AuthorizationUtils
{
    public static ApiKeyData? ParseApiKey(
        HttpContext httpContext,
        AppData app)
    {
        var authHeader = httpContext.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authHeader))
            return null;

        if (!authHeader.StartsWith("Bearer "))
            return null;
        
        var token = authHeader["Bearer ".Length..];
        var parts = token.Split('_');
        var prefix = parts[0];
        var key = parts[1];
        
        var id = key[..28];
        var secret = key[28..];

        var apiKeyData = app.GetApiKeyById(id);
        if (apiKeyData == null)
            return null;

        if (apiKeyData.Prefix != prefix)
            return null;
        
        if (apiKeyData.ExpiresAt.HasValue && apiKeyData.ExpiresAt.Value < DateTime.UtcNow)
            return null;
        
        var hash = $"{secret}{apiKeyData.Salt}".ToSha256();
        return hash == apiKeyData.Hash ? apiKeyData : null;
    }
}