namespace Routify.Api.Models.Consumers;

public record ConsumerInput
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? Alias { get; set; }
}