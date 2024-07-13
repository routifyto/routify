using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Gateway;
using Routify.Data;

namespace Routify.Api.Controllers;

[Route("/v1/gateway")]
public class GatewayController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet("data", Name = "GetGatewayData")]
    public async Task<ActionResult<GatewayDataOutput>> GetGatewayData()
    {
        var allApps = await databaseContext
            .Apps
            .AsNoTracking()
            .ToListAsync();

        var allRoutes = await databaseContext
            .Routes
            .AsNoTracking()
            .ToListAsync();

        var allRouteProviders = await databaseContext
            .RouteProviders
            .AsNoTracking()
            .ToListAsync();
        
        var allAppProviders = await databaseContext
            .AppProviders
            .AsNoTracking()
            .ToListAsync();
        
        var allApiKeys = await databaseContext
            .ApiKeys
            .AsNoTracking()
            .ToListAsync();

        var apps = allApps
            .Select(app => new GatewayAppOutput
            {
                Id = app.Id,
                Name = app.Name,
                Routes = allRoutes
                    .Where(route => route.AppId == app.Id)
                    .Select(route => new GatewayRouteOutput
                    {
                        Id = route.Id,
                        Name = route.Name,
                        Path = route.Path,
                        Type = route.Type,
                        Schema = route.Schema,
                        Providers = allRouteProviders
                            .Where(routeProvider => routeProvider.RouteId == route.Id)
                            .Select(routeProvider => new GatewayRouteProviderOutput
                            {
                                Id = routeProvider.Id,
                                AppProviderId = routeProvider.AppProviderId,
                                Model = routeProvider.Model,
                                Attrs = routeProvider.Attrs
                            })
                            .ToList()
                    })
                    .ToList(),
                Providers = allAppProviders
                    .Where(appProvider => appProvider.AppId == app.Id)
                    .Select(appProvider => new GatewayAppProviderOutput
                    {
                        Id = appProvider.Id,
                        Provider = appProvider.Provider,
                        Alias = appProvider.Alias,
                        Attrs = appProvider.Attrs
                    })
                    .ToList(),
                ApiKeys = allApiKeys
                    .Where(apiKey => apiKey.AppId == app.Id)
                    .Select(apiKey => new GatewayApiKeyOutput
                    {
                        Id = apiKey.Id,
                        Hash = apiKey.Hash,
                        Salt = apiKey.Salt,
                        Prefix = apiKey.Prefix,
                        Algorithm = apiKey.Algorithm,
                        ExpiresAt = apiKey.ExpiresAt
                    })
                    .ToList()
            })
            .ToList();

        return new GatewayDataOutput
        {
            Apps = apps
        };
    }

    [HttpPost("logs", Name = "CreateGatewayLogs")]
    public async Task<ActionResult> CreateGatewayLogs(
        [FromBody] GatewayLogsInput input,
        CancellationToken cancellationToken = default)
    {
        await databaseContext.CompletionLogs.AddRangeAsync(input.CompletionLogs, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        return Ok();
    }
}