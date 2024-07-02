using Routify.Data.Models;

namespace Routify.Api.Models.AppUsers;

public class AppUserPayload
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Avatar { get; set; }
    public AppUserRole Role { get; set; }
    public DateTime CreatedAt { get; set; }
}