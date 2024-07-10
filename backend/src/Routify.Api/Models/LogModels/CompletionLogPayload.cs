namespace Routify.Api.Models.LogModels;

public class CompletionLogPayload
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
    
    public string RequestBody { get; set; } = null!;
    
    public int ResponseStatusCode { get; set; }
    public string ResponseBody { get; set; } = null!;
    
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    
    public decimal InputCost { get; set; }
    public decimal OutputCost { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }
    public double Duration { get; set; }
    
    public LogRoutePayload? Route { get; set; }
    public LogAppProviderPayload? AppProvider { get; set; }
}