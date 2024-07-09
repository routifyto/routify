namespace Routify.Gateway.Models.Data;

internal class AppProviderData
{
    public string Id { get; set; } = null!;
    public string Provider { get; set; } = null!;
    public string Alias { get; set; } = null!;
    public Dictionary<string, string> Attrs { get; set; } = [];
}