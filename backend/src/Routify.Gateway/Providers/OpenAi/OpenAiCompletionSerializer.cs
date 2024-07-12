using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.OpenAi.Models;

namespace Routify.Gateway.Providers.OpenAi;

internal class OpenAiCompletionSerializer : ICompletionSerializer
{
    public ICompletionInput? Parse(
        string input)
    {
        return JsonSerializer.Deserialize<OpenAiCompletionInput>(input);
    }

    public string Serialize(
        ICompletionOutput output,
        JsonSerializerOptions? options = default)
    {
        if (output is not OpenAiCompletionOutput openAiOutput)
        {
            throw new ArgumentException("Invalid output type");
        }
        
        return JsonSerializer.Serialize(openAiOutput, options);
    }
}