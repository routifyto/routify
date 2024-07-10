namespace Routify.Provider.Core.Completion;

public interface ICompletionPayloadSerializer
{
    string Serialize(CompletionPayload payload);
}