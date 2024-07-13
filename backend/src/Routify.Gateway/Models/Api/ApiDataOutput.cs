namespace Routify.Gateway.Models.Api;

internal record ApiDataOutput
{
    public List<ApiAppOutput> Apps { get; set; } = [];
}