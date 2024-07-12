using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.MistralAi.Models;

namespace Routify.Gateway.Providers.MistralAi;

internal class MistralAiCompletionSerializer : ICompletionSerializer
{
    public ICompletionInput? Parse(
        string input)
    {
        return JsonSerializer.Deserialize<MistralAiCompletionInput>(input);
    }

    public string Serialize(
        ICompletionOutput output,
        JsonSerializerOptions? options = default)
    {
        if (output is not MistralAiCompletionOutput mistralAiOutput)
        {
            throw new ArgumentException("Invalid output type");
        }
        
        return JsonSerializer.Serialize(mistralAiOutput, options);
    }
}