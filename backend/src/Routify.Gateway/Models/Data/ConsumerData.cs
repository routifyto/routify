namespace Routify.Gateway.Models.Data;

internal record ConsumerData
{
    public string Id { get; set; } = null!;
    public string? Alias { get; set; }
}