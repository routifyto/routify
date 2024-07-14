namespace Routify.Api.Models.Consumers;

public record ConsumerOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Alias { get; set; }
}