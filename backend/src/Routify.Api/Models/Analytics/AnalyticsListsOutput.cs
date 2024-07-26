namespace Routify.Api.Models.Analytics;

public record AnalyticsListsOutput
{
    public List<MetricsOutput> Providers { get; set; } = null!;
    public List<MetricsOutput> Models { get; set; } = null!;
    public List<MetricsOutput> Consumers { get; set; } = null!;
}