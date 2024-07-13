namespace Routify.Api.Models.Analytics;

public record MetricsOutput
{
    public string Id { get; set; } = null!;
    public int TotalRequests { get; set; }
    public int TotalTokens { get; set; }
    public decimal TotalCost { get; set; }
    public double AverageDuration { get; set; }
}