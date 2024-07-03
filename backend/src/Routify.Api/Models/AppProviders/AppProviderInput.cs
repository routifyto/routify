namespace Routify.Api.Models.AppProviders;

public record AppProviderInput
{
    public string Name { get; set; } = null!;
    public string Alias { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string? Description { get; set; }
    public Dictionary<string, string> Attrs { get; set; } = [];
}