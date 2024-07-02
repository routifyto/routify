namespace Routify.Api.Models.Accounts;

public record EmailLoginInput
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}