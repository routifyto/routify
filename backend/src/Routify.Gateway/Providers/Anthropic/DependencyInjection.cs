using Routify.Core.Constants;
using Routify.Gateway.Abstractions;

namespace Routify.Gateway.Providers.Anthropic;

internal static class DependencyInjection
{
    public static void AddAnthropic(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.Anthropic, client =>
        {
            client.BaseAddress = new Uri("https://api.anthropic.com/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, AnthropicCompletionProvider>(ProviderIds.Anthropic);
    }
}