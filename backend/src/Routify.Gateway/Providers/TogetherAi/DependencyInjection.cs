using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.TogetherAi;

internal static class DependencyInjection
{
    public static void AddTogetherAi(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.TogetherAi, client =>
        {
            client.BaseAddress = new Uri("https://api.together.xyz/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, TogetherAiCompletionProvider>(ProviderIds.TogetherAi);
    }
}