using Routify.Data.Common;

namespace Routify.Gateway.Abstractions;

internal record CompletionResponse
{
    public int StatusCode { get; set; }
    public ICompletionOutput? Output { get; set; }
    public RequestLog? RequestLog { get; set; }
    public ResponseLog? ResponseLog { get; set; }
    public string? Model { get; set; }
    public string? Error { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public decimal InputCost { get; set; }
    public decimal OutputCost { get; set; }
}