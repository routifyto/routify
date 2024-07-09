namespace Routify.Gateway.Models.Api;

internal record ApiDataPayload
{
    public List<ApiAppPayload> Apps { get; set; } = [];
}