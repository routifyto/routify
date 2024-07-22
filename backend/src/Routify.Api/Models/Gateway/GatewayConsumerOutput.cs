using Routify.Data.Common;

namespace Routify.Api.Models.Gateway;

public record GatewayConsumerOutput
{
    public string Id { get; set; } = null!;
    public string? Alias { get; set; }
    public CostLimitConfig? CostLimitConfig { get; set; }
}