using Routify.Data.Models;

namespace Routify.Api.Models.AppUsers;

public record AppUsersInput
{
    public List<string> Emails { get; set; } = null!;
    public AppUserRole Role { get; set; }
}