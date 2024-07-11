using Routify.Gateway.Models.Api;

namespace Routify.Gateway.Models.Data;

internal record AppData
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;

    private Dictionary<string, RouteData> Routes { get; set; } = [];
    private Dictionary<string, AppProviderData> Providers { get; set; } = [];

    public AppData(
        ApiAppPayload payload)
    {
        Id = payload.Id;
        Name = payload.Name;
        Routes = payload
            .Routes
            .ToDictionary(route => route.Path, route => new RouteData
            {
                Id = route.Id,
                Name = route.Name,
                Path = route.Path,
                Type = route.Type,
                Schema = route.Schema,
                Providers = route
                    .Providers
                    .Select(routeProvider => new RouteProviderData
                    {
                        Id = routeProvider.Id,
                        AppProviderId = routeProvider.AppProviderId,
                        Model = routeProvider.Model,
                        Attrs = routeProvider.Attrs
                    })
                    .ToList()
            });
        
        Providers = payload
            .Providers
            .ToDictionary(provider => provider.Id, provider => new AppProviderData
            {
                Id = provider.Id,
                Provider = provider.Provider,
                Alias = provider.Alias,
                Attrs = provider.Attrs
            });
    }

    public RouteData? GetRoute(
        string path)
    {
        Routes.TryGetValue(path, out var route);
        return route;
    }
    
    public AppProviderData? GetProviderById(
        string id)
    {
        Providers.TryGetValue(id, out var provider);
        return provider;
    }
}