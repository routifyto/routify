namespace Routify.Gateway.Models.Api;

internal record ApiAppOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<ApiRouteOutput> Routes { get; set; } = [];
    public List<ApiAppProviderOutput> Providers { get; set; } = [];
    public List<ApiApiKeyOutput> ApiKeys { get; set; } = [];
}