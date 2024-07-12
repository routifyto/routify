namespace Routify.Provider.Core.Models;

public record CompletionChoicePayload
{
    public int? Index { get; set; }
    
    public string? FinishReason { get; set; }
    
    public CompletionMessagePayload Message { get; set; } = null!;
    
    public CompletionLogprobsPayload? Logprobs { get; set; }
}