using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionSerializer : ICompletionSerializer
{
    public ICompletionInput? Parse(
        string input)
    {
        return JsonSerializer.Deserialize<TogetherAiCompletionInput>(input);
    }

    public string Serialize(
        ICompletionOutput output, 
        JsonSerializerOptions? options = default)
    {
        if (output is not TogetherAiCompletionOutput togetherAiOutput)
        {
            throw new ArgumentException("Invalid output type");
        }
        
        return JsonSerializer.Serialize(togetherAiOutput, options);
    }
}