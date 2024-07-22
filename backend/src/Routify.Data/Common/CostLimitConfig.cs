namespace Routify.Data.Common;

public record CostLimitConfig
{
    public bool Enabled { get; set; }
    public decimal? DailyLimit { get; set; }
    public decimal? MonthlyLimit { get; set; }
}