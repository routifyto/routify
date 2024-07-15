using Routify.Gateway.Models.Data;

namespace Routify.Gateway.Abstractions;

internal record CompletionRequest
{
    public ICompletionInput Input { get; set; } = null!;
    public AppProviderData AppProvider { get; set; } = null!;
    public RouteProviderData RouteProvider { get; set; } = null!;
}