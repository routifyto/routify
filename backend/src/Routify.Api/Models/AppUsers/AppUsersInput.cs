using Routify.Data.Enums;

namespace Routify.Api.Models.AppUsers;

public record AppUsersInput
{
    public List<string> Emails { get; set; } = null!;
    public AppRole Role { get; set; }
}