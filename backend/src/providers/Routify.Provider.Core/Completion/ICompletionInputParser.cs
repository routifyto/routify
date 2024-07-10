namespace Routify.Provider.Core.Completion;

public interface ICompletionInputParser
{
    CompletionInput? Parse(string input);
}