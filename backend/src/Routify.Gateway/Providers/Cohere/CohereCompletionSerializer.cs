using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Cohere.Models;

namespace Routify.Gateway.Providers.Cohere;

internal class CohereCompletionSerializer : ICompletionSerializer
{
    public ICompletionInput? Parse(
        string input)
    {
        return JsonSerializer.Deserialize<CohereCompletionInput>(input);
    }

    public string Serialize(
        ICompletionOutput output,
        JsonSerializerOptions? options = default)
    {
        if (output is not CohereCompletionOutput cohereOutput)
        {
            throw new ArgumentException("Invalid output type");
        }
        
        return JsonSerializer.Serialize(cohereOutput, options);
    }
}