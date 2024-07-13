using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Cohere;

internal static class DependencyInjection
{
    public static void AddOpenAi(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.Cohere, client =>
        {
            client.BaseAddress = new Uri("https://api.cohere.com/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, CohereCompletionProvider>(ProviderIds.Cohere);
        services.AddKeyedScoped<ICompletionInputMapper, CohereCompletionInputMapper>(ProviderIds.Cohere);
        services.AddKeyedScoped<ICompletionOutputMapper, CohereCompletionOutputMapper>(ProviderIds.Cohere);
        services.AddKeyedScoped<ICompletionSerializer, CohereCompletionSerializer>(ProviderIds.Cohere);
    }
}