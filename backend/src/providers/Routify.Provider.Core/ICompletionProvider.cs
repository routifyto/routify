using Routify.Provider.Core.Models;

namespace Routify.Provider.Core;

public interface ICompletionProvider
{
    Task<CompletionResponse> CompleteAsync(CompletionRequest request, CancellationToken cancellationToken);
}