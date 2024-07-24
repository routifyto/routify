namespace Routify.Api.Models.LogModels;

public class CompletionOutgoingLogOutput
{
    public string Id { get; set; } = null!;
    public string IncomingLogId { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string AppProviderId { get; set; } = null!;
    public string RouteProviderId { get; set; } = null!;
    public int RetryCount { get; set; }
    
    public string? RequestUrl { get; set; }
    public string? RequestMethod { get; set; }
    public Dictionary<string, string>? RequestHeaders { get; set; }
    public string? RequestBody { get; set; }
    
    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    public Dictionary<string, string>? ResponseHeaders { get; set; }
    
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public double Duration { get; set; }
    
    public LogAppProviderOutput? AppProvider { get; set; }
}