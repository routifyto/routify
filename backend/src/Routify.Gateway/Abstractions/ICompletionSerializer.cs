using System.Text.Json;

namespace Routify.Gateway.Abstractions;

internal interface ICompletionSerializer
{
    ICompletionInput? Parse(string input);
    string Serialize(ICompletionOutput output, JsonSerializerOptions? options = default);
}