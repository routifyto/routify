namespace Routify.Gateway.Abstractions;

internal interface ICompletionProvider
{
    string Id { get; }
    Dictionary<string, CompletionModel> Models { get; }
    Task<CompletionResponse> CompleteAsync(CompletionRequest request, CancellationToken cancellationToken);
    ICompletionInput? ParseInput(string input);
    string SerializeOutput(ICompletionOutput output);
}