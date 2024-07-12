namespace Routify.Api.Models.Analytics;

public record AnalyticsHistogramPayload
{
    public List<DateTimeHistogramPayload> Requests { get; set; } = [];
}