using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Perplexity;

internal static class DependencyInjection
{
    public static void AddPerplexity(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.Perplexity, client =>
        {
            client.BaseAddress = new Uri("https://api.perplexity.ai/");
        });

        services.AddKeyedScoped<ICompletionProvider, PerplexityCompletionProvider>(ProviderIds.Perplexity);
    }
}