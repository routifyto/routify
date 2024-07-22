using Routify.Data.Common;
using Routify.Data.Enums;

namespace Routify.Gateway.Models.Api;

public record ApiApiKeyOutput
{
    public string Id { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Prefix { get; set; } = null!;
    public ApiKeyHashAlgorithm Algorithm { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public CostLimitConfig? CostLimitConfig { get; set; }
}