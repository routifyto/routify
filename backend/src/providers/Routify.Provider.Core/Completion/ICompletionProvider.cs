namespace Routify.Provider.Core.Completion;

public interface ICompletionProvider
{
    Task<CompletionResponse> CompleteAsync(CompletionRequest request, CancellationToken cancellationToken);
}