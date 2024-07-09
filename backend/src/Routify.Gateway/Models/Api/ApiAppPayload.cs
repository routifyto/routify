namespace Routify.Gateway.Models.Api;

internal record ApiAppPayload
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<ApiRoutePayload> Routes { get; set; } = [];
    public List<ApiAppProviderPayload> Providers { get; set; } = [];
}