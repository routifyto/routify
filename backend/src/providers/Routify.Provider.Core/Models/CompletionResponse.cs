namespace Routify.Provider.Core.Models;

public record CompletionResponse
{
    public int StatusCode { get; set; }
    public CompletionPayload? Payload { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public decimal InputCost { get; set; }
    public decimal OutputCost { get; set; }
}