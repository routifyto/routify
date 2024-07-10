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
    public async Task<ActionResult<GatewayDataPayload>> GetGatewayData()
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

        var apps = allApps
            .Select(app => new GatewayAppPayload
            {
                Id = app.Id,
                Name = app.Name,
                Routes = allRoutes
                    .Where(route => route.AppId == app.Id)
                    .Select(route => new GatewayRoutePayload
                    {
                        Id = route.Id,
                        Name = route.Name,
                        Path = route.Path,
                        Type = route.Type,
                        InputType = route.InputType,
                        Providers = allRouteProviders
                            .Where(routeProvider => routeProvider.RouteId == route.Id)
                            .Select(routeProvider => new GatewayRouteProviderPayload
                            {
                                Id = routeProvider.Id,
                                AppProviderId = routeProvider.AppProviderId,
                                Model = routeProvider.Model
                            })
                            .ToList()
                    })
                    .ToList(),
                Providers = allAppProviders
                    .Where(appProvider => appProvider.AppId == app.Id)
                    .Select(appProvider => new GatewayAppProviderPayload
                    {
                        Id = appProvider.Id,
                        Provider = appProvider.Provider,
                        Alias = appProvider.Alias,
                        Attrs = appProvider.Attrs
                    })
                    .ToList()
            })
            .ToList();

        return new GatewayDataPayload
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