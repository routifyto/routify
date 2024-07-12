namespace Routify.Gateway.Abstractions;

internal interface ICompletionProvider
{
    Task<CompletionResponse> CompleteAsync(CompletionRequest request, CancellationToken cancellationToken);
}