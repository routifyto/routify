using Routify.Data.Enums;
using Routify.Data.Models;

namespace Routify.Gateway.Abstractions;

internal record CompletionResponse
{
    public int StatusCode { get; set; }
    public ICompletionOutput? Output { get; set; }
    public string? ResponseBody { get; set; }
    public Dictionary<string, string>? ResponseHeaders { get; set; }
    public string? Model { get; set; }
    public string? Error { get; set; }
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    public decimal InputCost { get; set; }
    public decimal OutputCost { get; set; }
    public CacheStatus CacheStatus { get; set; }
    
    public CompletionOutgoingLog? Log { get; set; }
}