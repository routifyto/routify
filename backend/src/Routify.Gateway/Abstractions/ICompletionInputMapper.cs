namespace Routify.Gateway.Abstractions;

internal interface ICompletionInputMapper
{
    ICompletionInput Map(ICompletionInput input);
}