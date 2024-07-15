using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.OpenAi;

internal static class DependencyInjection
{
    public static void AddOpenAi(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.OpenAi, client =>
        {
            client.BaseAddress = new Uri("https://api.openai.com/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, OpenAiCompletionProvider>(ProviderIds.OpenAi);
    }
}