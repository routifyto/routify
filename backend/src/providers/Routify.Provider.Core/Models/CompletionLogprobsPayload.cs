namespace Routify.Provider.Core.Models;

public record CompletionLogprobsPayload
{
    public List<CompletionLogprobsContentPayload>? Content { get; set; }
}