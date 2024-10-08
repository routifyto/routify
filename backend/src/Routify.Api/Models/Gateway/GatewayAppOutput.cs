namespace Routify.Api.Models.Gateway;

public record GatewayAppOutput
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<GatewayRouteOutput> Routes { get; set; } = [];
    public List<GatewayAppProviderOutput> Providers { get; set; } = [];
    public List<GatewayApiKeyOutput> ApiKeys { get; set; } = [];
    public List<GatewayConsumerOutput> Consumers { get; set; } = [];
}