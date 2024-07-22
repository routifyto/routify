using Routify.Data.Enums;

namespace Routify.Api.Models.LogModels;

public record CompletionLogOutput
{
    public string Id { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? Provider { get; set; }
    public string? Model { get; set; }
    public string? AppProviderId { get; set; }
    public string? RouteProviderId { get; set; } = null!;
    public string ApiKeyId { get; set; } = null!;
    public string? SessionId { get; set; }
    public string? ConsumerId { get; set; }
    public int OutgoingRequestsCount { get; set; }
    public CacheStatus CacheStatus { get; set; }
    
    public string? RequestUrl { get; set; }
    public string? RequestMethod { get; set; }
    public Dictionary<string, string>? RequestHeaders { get; set; }
    public string? RequestBody { get; set; }
    public int StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    public Dictionary<string, string>? ResponseHeaders { get; set; }
    
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    
    public decimal InputCost { get; set; }
    public decimal OutputCost { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public double Duration { get; set; }
    
    public LogRouteOutput? Route { get; set; }
    public LogAppProviderOutput? AppProvider { get; set; }
}