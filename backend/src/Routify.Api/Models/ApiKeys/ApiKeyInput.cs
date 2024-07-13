using Routify.Data.Enums;

namespace Routify.Api.Models.ApiKeys;

public record ApiKeyInput
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool CanUseGateway { get; set; }
    public AppRole? Role { get; set; }
    public DateTime? ExpiresAt { get; set; }
}