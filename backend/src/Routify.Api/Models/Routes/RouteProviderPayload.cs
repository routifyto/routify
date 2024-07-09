namespace Routify.Api.Models.Routes;

public record RouteProviderPayload
{
    public string Id { get; set; } = null!;
    public string AppProviderId { get; set; } = null!;
    public string? Model { get; set; }
}