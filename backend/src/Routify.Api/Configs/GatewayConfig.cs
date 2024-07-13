namespace Routify.Api.Configs;

public record GatewayConfig
{
    public List<string> Tokens { get; set; } = [];
}