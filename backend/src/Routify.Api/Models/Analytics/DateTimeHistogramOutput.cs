namespace Routify.Api.Models.Analytics;

public record DateTimeHistogramOutput
{
    public DateTime DateTime { get; set; }
    public long Count { get; set; }
}