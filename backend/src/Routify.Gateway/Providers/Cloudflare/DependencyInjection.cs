using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Cloudflare;

internal static class DependencyInjection
{
    public static void AddCloudflare(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.Cloudflare, client =>
        {
            client.BaseAddress = new Uri("https://api.cloudflare.com/client/v4/accounts/");
        });

        services.AddKeyedScoped<ICompletionProvider, CloudflareCompletionProvider>(ProviderIds.Cloudflare);
    }
}