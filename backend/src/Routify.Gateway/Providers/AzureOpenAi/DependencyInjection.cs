using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.AzureOpenAi;

internal static class DependencyInjection
{
    public static void AddAzureOpenAi(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.AzureOpenAi);
        services.AddKeyedScoped<ICompletionProvider, AzureOpenAiCompletionProvider>(ProviderIds.AzureOpenAi);
    }
}