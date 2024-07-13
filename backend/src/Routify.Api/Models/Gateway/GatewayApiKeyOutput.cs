using Routify.Data.Enums;

namespace Routify.Api.Models.Gateway;

public record GatewayApiKeyOutput
{
    public string Id { get; set; } = null!;
    public string Hash { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string Prefix { get; set; } = null!;
    public ApiKeyHashAlgorithm Algorithm { get; set; }
    public DateTime? ExpiresAt { get; set; }
}