namespace Routify.Gateway.Abstractions;

internal interface ICompletionInputParser
{
    ICompletionInput? Parse(string input);
}