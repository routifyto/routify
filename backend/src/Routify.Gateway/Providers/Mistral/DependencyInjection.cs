using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Mistral;

internal static class DependencyInjection
{
    public static void AddMistral(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.Mistral, client =>
        {
            client.BaseAddress = new Uri("https://api.mistral.ai/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, MistralCompletionProvider>(ProviderIds.Mistral);
    }
}