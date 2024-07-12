namespace Routify.Provider.Core.Models;

public record CompletionLogprobsContentPayload
{
    public string Token { get; set; } = null!;
    public float? Logprob { get; set; }
    public List<byte>? Bytes { get; set; }
   public List<CompletionLogprobsContentPayload> TopLogprobs { get; set; } = [];
}