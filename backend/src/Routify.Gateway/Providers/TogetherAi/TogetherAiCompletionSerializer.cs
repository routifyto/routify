using System.Text.Json;
using Routify.Gateway.Abstractions;
using Routify.Gateway.Providers.TogetherAi.Models;

namespace Routify.Gateway.Providers.TogetherAi;

internal class TogetherAiCompletionSerializer : ICompletionInputParser
{
    public ICompletionInput? Parse(
        string input)
    {
        return JsonSerializer.Deserialize<TogetherAiCompletionInput>(input);
    }
}