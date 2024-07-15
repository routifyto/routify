using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Groq;

internal static class DependencyInjection
{
    public static void AddGroq(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.OpenAi, client =>
        {
            client.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, GroqCompletionProvider>(ProviderIds.OpenAi);
    }
}