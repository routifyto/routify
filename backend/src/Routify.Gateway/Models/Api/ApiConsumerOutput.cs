using Routify.Data.Common;

namespace Routify.Gateway.Models.Api;

internal record ApiConsumerOutput
{
    public string Id { get; set; } = null!;
    public string? Alias { get; set; }
    public CostLimitConfig? CostLimitConfig { get; set; }
}