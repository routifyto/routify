using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.Anthropic.Models;

namespace Routify.Gateway.Providers.Anthropic;

internal class AnthropicCompletionSerializer : ICompletionSerializer
{
    public ICompletionInput? Parse(
        string input)
    {
        return JsonSerializer.Deserialize<AnthropicCompletionInput>(input);
    }

    public string Serialize(
        ICompletionOutput output,
        JsonSerializerOptions? options = default)
    {
        if (output is not AnthropicCompletionOutput anthropicCompletionOutput)
        {
            throw new ArgumentException("Invalid output type");
        }
        
        return JsonSerializer.Serialize(anthropicCompletionOutput, options);
    }
}