namespace Routify.Api.Models.LogModels;

public class LogAppProviderPayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Alias { get; set; } = null!;
    public string? Description { get; set; }
}