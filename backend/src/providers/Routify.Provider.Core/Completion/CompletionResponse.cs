namespace Routify.Provider.Core.Completion;

public record CompletionResponse
{
    public int StatusCode { get; set; }
    public CompletionPayload? Payload { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public double InputCost { get; set; }
    public double OutputCost { get; set; }
}