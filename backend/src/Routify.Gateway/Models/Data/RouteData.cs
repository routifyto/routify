using Routify.Data.Enums;
using Routify.Gateway.Models.Api;

namespace Routify.Gateway.Models.Data;

internal record RouteData
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public RouteType Type { get; set; }
    public string Schema { get; set; } = null!;
    public RouteStrategy Strategy { get; set; }
    public List<RouteProviderData> Providers { get; set; } = [];
    public int TotalWeight { get; set; }

    public RouteData(
        ApiRouteOutput output)
    {
        Id = output.Id;
        Name = output.Name;
        Path = output.Path;
        Type = output.Type;
        Schema = output.Schema;
        Strategy = output.Strategy;
        
        TotalWeight = output.Providers.Sum(x => x.Weight);
        
        var weightFrom = 0;
        foreach (var routeProviderOutput in output.Providers)
        {
            var weight = Math.Max(routeProviderOutput.Weight, 1);
            Providers.Add(new RouteProviderData
            {
                Id = routeProviderOutput.Id,
                AppProviderId = routeProviderOutput.AppProviderId,
                Model = routeProviderOutput.Model,
                Attrs = routeProviderOutput.Attrs,
                WeightFrom = weightFrom,
                WeightTo = weightFrom + weight
            });
            
            weightFrom += weight;
        }
    }
}