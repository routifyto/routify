using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.MistralAi;

internal static class DependencyInjection
{
    public static void AddMistralAi(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.MistralAi, client =>
        {
            client.BaseAddress = new Uri("https://api.mistral.ai/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, MistralAiCompletionProvider>(ProviderIds.MistralAi);
    }
}