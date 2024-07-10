using System.Text.Json;

namespace Routify.Provider.Core.Completion;

public interface ICompletionSerializer
{
    CompletionInput? Parse(string input);
    string Serialize(CompletionPayload payload, JsonSerializerOptions? options = default);
}