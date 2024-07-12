namespace Routify.Api.Models.Analytics;

public record AnalyticsListsPayload
{
    public List<MetricsPayload> Providers { get; set; } = null!;
    public List<MetricsPayload> Models { get; set; } = null!;
}