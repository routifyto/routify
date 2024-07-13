namespace Routify.Api.Models.Gateway;

public record GatewayDataOutput
{
    public List<GatewayAppOutput> Apps { get; set; } = [];
}