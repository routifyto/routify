namespace Routify.Gateway.Abstractions;

internal interface ICompletionProvider
{
    string Id { get; }
    Task<CompletionResponse> CompleteAsync(CompletionRequest request, CancellationToken cancellationToken);
    ICompletionInput? ParseInput(string input);
    string SerializeOutput(ICompletionOutput output);
    
}