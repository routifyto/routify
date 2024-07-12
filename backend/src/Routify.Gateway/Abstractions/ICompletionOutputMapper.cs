namespace Routify.Gateway.Abstractions;

internal interface ICompletionOutputMapper
{
    ICompletionOutput Map(ICompletionOutput output);
}