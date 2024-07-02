namespace Routify.Api.Models.Accounts;

public record EmailRegisterInput
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}