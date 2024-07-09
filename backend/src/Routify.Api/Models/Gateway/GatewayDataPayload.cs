namespace Routify.Api.Models.Gateway;

public record GatewayDataPayload
{
    public List<GatewayAppPayload> Apps { get; set; } = [];
}