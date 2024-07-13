using Routify.Data.Enums;

namespace Routify.Api.Models.ApiKeys;

public record ApiKeyOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool CanUseGateway { get; set; }
    public AppRole? Role { get; set; }
    public string Prefix { get; set; } = null!;
    public string Suffix { get; set; } = null!;
    public DateTime? ExpiresAt { get; set; }
}