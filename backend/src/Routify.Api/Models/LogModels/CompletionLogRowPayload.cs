namespace Routify.Api.Models.LogModels;

public class CompletionLogRowPayload
{
    public string Id { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? Provider { get; set; }
    public string? Model { get; set; }
    
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    
    public decimal InputCost { get; set; }
    public decimal OutputCost { get; set; }
    
    public DateTime EndedAt { get; set; }
    public double Duration { get; set; }
}