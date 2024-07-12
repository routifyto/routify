namespace Routify.Api.Models.Analytics;

public record DateTimeHistogramPayload
{
    public DateTime DateTime { get; set; }
    public long Count { get; set; }
}