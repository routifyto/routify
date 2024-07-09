namespace Routify.Api.Models.LogModels;

public class TextLogRowPayload
{
    public string Id { get; set; } = null!;
    public string RouteId { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? Provider { get; set; }
    public string? Model { get; set; }
    
    public int InputTokens { get; set; }
    public int OutputTokens { get; set; }
    
    public double InputCost { get; set; }
    public double OutputCost { get; set; }
    
    public DateTime EndedAt { get; set; }
    public double Duration { get; set; }
}