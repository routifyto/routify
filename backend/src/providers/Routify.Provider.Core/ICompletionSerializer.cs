using System.Text.Json;
using Routify.Provider.Core.Models;

namespace Routify.Provider.Core;

public interface ICompletionSerializer
{
    CompletionInput? Parse(string input);
    string Serialize(CompletionPayload payload, JsonSerializerOptions? options = default);
}