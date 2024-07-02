namespace Routify.Api.Models.Apps;

public record AppPayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}