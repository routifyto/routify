using Microsoft.Extensions.DependencyInjection;
using Routify.Core.Constants;
using Routify.Provider.Core.Completion;

namespace Routify.Provider.OpenAi;

public static class DependencyInjection
{
    public static void AddOpenAi(
        this IServiceCollection services)
    {
        services.AddHttpClient(ProviderIds.OpenAi, client =>
        {
            client.BaseAddress = new Uri("https://api.openai.com/v1/");
        });

        services.AddKeyedScoped<ICompletionProvider, OpenAiCompletionProvider>(ProviderIds.OpenAi);
        services.AddKeyedScoped<ICompletionInputParser, OpenAiCompletionInputParser>(ProviderIds.OpenAi);
        services.AddKeyedScoped<ICompletionPayloadSerializer, OpenAiCompletionPayloadSerializer>(ProviderIds.OpenAi);
    }
}