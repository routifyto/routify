namespace Routify.Api.Models.Analytics;

public record AnalyticsHistogramOutput
{
    public List<DateTimeHistogramOutput> Requests { get; set; } = [];
}