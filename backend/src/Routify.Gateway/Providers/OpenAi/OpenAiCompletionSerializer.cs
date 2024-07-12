using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.OpenAi.Models;

namespace Routify.Gateway.Providers.OpenAi;

internal class OpenAiCompletionSerializer : ICompletionInputParser
{
    public ICompletionInput? Parse(
        string input)
    {
        return JsonSerializer.Deserialize<OpenAiCompletionInput>(input);
    }
}