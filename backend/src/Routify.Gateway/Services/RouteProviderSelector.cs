using Routify.Gateway.Models.Data;
using RouteData = Routify.Gateway.Models.Data.RouteData;

namespace Routify.Gateway.Services;

internal class RouteProviderSelector(RouteData routeData)
{
    private readonly HashSet<string> _usedRouteProviderIds = [];
    private readonly Random _random = new();
    
    public bool HasNextProvider => _usedRouteProviderIds.Count < routeData.Providers.Count;
    
    public RouteProviderData? GetNextProvider()
    {
        if (!HasNextProvider)
            return null;
        
        var availableProviders = routeData.Providers
            .Where(x => !_usedRouteProviderIds.Contains(x.Id))
            .OrderBy(x => x.Index)
            .ToList();
        
        if (!routeData.IsLoadBalanceEnabled)
            return availableProviders.FirstOrDefault();
        
        var totalWeight = availableProviders.Sum(x => x.Weight);
        var randomWeight = _random.Next(totalWeight);
        
        var cumulativeWeight = 0;
        foreach (var provider in availableProviders)
        {
            cumulativeWeight += provider.Weight;
            if (randomWeight >= cumulativeWeight) 
                continue;
            
            _usedRouteProviderIds.Add(provider.Id);
            return provider;
        }
        
        return null;
    }
}