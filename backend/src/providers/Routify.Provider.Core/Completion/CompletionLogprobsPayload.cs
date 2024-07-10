namespace Routify.Provider.Core.Completion;

public record CompletionLogprobsPayload
{
    public List<CompletionLogprobsContentPayload>? Content { get; set; }
}