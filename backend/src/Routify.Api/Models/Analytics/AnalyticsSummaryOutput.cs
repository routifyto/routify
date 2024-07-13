namespace Routify.Api.Models.Analytics;

public record AnalyticsSummaryOutput
{
    public int TotalRequests { get; set; }
    public int PreviousTotalRequests { get; set; }
    public int TotalTokens { get; set; }
    public int PreviousTotalTokens { get; set; }
    public decimal TotalCost { get; set; }
    public decimal PreviousTotalCost { get; set; }
    public double AverageDuration { get; set; }
    public double PreviousAverageDuration { get; set; }
}
