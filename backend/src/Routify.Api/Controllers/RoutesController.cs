using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Common;
using Routify.Api.Models.Routes;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Common;
using Routify.Data.Models;
using Route = Routify.Data.Models.Route;

namespace Routify.Api.Controllers;

[Route("/v1/apps/{appId}/routes")]
public class RoutesController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet(Name = "GetRoutes")]
    public async Task<ActionResult<PaginatedPayload<RoutePayload>>> GetRoutesAsync(
        [FromRoute] string appId,
        [FromQuery] string? after,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var query = databaseContext
            .Routes
            .Include(x => x.Providers)
            .Where(x => x.AppId == appId);

        if (!string.IsNullOrWhiteSpace(after))
            query = query.Where(x => x.Id.CompareTo(after) > 0);

        // Limit the number of items to fetch
        limit = Math.Max(1, Math.Min(limit, 100));
        var routes = await query
            .OrderBy(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var routePayloads = routes
            .Select(MapToPayload)
            .ToList();

        var hasNext = routes.Count == limit;
        var nextCursor = hasNext ? routes.Last().Id : null;
        var payload = new PaginatedPayload<RoutePayload>
        {
            Items = routePayloads,
            HasNext = hasNext,
            NextCursor = nextCursor
        };

        return Ok(payload);
    }
    
    [HttpGet("{routeId}", Name = "GetRoute")]
    public async Task<ActionResult<RoutePayload>> GetRouteAsync(
        [FromRoute] string appId,
        [FromRoute] string routeId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();
        
        var route = await databaseContext
            .Routes
            .Include(x => x.Providers)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);
        
        if (route is null)
            return NotFound();
        
        var payload = MapToPayload(route);
        return Ok(payload);
    }
    
    [HttpPost(Name = "CreateRoute")]
    public async Task<ActionResult<RoutePayload>> CreateRouteAsync(
        [FromRoute] string appId,
        [FromBody] CreateRouteInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var route = new Route
        {
            Id = RoutifyId.Generate(IdType.Route),
            AppId = appId,
            Name = input.Name,
            Description = input.Description,
            Path = input.Path,
            Type = input.Type,
            Attrs = input.Attrs,
            Config = new RouteConfig(),
            CreatedAt = DateTime.UtcNow,
            CreatedBy = CurrentUserId,
            VersionId = RoutifyId.Generate(IdType.Version)
        };
        
        var routeProviders = input
            .Providers
            .Select(x => new RouteProvider
            {
                Id = RoutifyId.Generate(IdType.RouteModel),
                RouteId = route.Id,
                AppId = route.AppId,
                AppProviderId = x.AppProviderId,
                Model = x.Model,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = CurrentUserId,
                VersionId = RoutifyId.Generate(IdType.Version)
            })
            .ToList();

        databaseContext.Routes.Add(route);
        databaseContext.RouteProviders.AddRange(routeProviders);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var routePayload = MapToPayload(route);
        return CreatedAtRoute("GetRoute", new { appId, routeId = route.Id }, routePayload);
    }
    
    [HttpPut("{routeId}", Name = "UpdateRoute")]
    public async Task<ActionResult<RoutePayload>> UpdateRouteAsync(
        [FromRoute] string appId,
        [FromRoute] string routeId,
        [FromBody] UpdateRouteInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var route = await databaseContext
            .Routes
            .Include(x => x.Providers)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);

        if (route is null)
            return NotFound();

        route.Name = input.Name;
        route.Description = input.Description;
        route.Path = input.Path;
        route.Attrs = input.Attrs;

        var routeProviderIds = input.Providers.Select(x => x.Id).ToList();
        foreach (var routeProvider in route.Providers)
        {
            if (!routeProviderIds.Contains(routeProvider.Id))
            {
                databaseContext.Remove(routeProvider);
            }
        }

        for (var i = 0; i < input.Providers.Count; i++)
        {
            var routeProviderInput = input.Providers[i];
            if (string.IsNullOrWhiteSpace(routeProviderInput.Id))
            {
                var routeProvider = new RouteProvider
                {
                    Id = RoutifyId.Generate(IdType.RouteModel),
                    RouteId = route.Id,
                    AppId = route.AppId,
                    AppProviderId = routeProviderInput.AppProviderId,
                    Model = routeProviderInput.Model,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = CurrentUserId,
                    VersionId = RoutifyId.Generate(IdType.Version)
                };
                
                databaseContext.RouteProviders.Add(routeProvider);
            }
            else
            {
                var routeProvider = route.Providers.SingleOrDefault(x => x.Id == routeProviderInput.Id);
                if (routeProvider is null)
                    throw new Exception("Route provider not found");

                routeProvider.AppProviderId = routeProviderInput.AppProviderId;
                routeProvider.Model = routeProviderInput.Model;
                routeProvider.UpdatedAt = DateTime.UtcNow;
                routeProvider.UpdatedBy = CurrentUserId;
                routeProvider.VersionId = RoutifyId.Generate(IdType.Version);
            }
        }
        
        await databaseContext.SaveChangesAsync(cancellationToken);

        var routePayload = MapToPayload(route);
        return Ok(routePayload);
    }
    
    [HttpDelete("{routeId}", Name = "DeleteRoute")]
    public async Task<ActionResult<DeletePayload>> DeleteRouteAsync(
        [FromRoute] string appId,
        [FromRoute] string routeId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var app = currentAppUser.App;
        if (app is null)
            return NotFound();

        var route = await databaseContext
            .Routes
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);

        if (route is null)
            return NotFound();

        databaseContext.Routes.Remove(route);
        await databaseContext.SaveChangesAsync(cancellationToken);

        return Ok(new DeletePayload());
    }

    private static RoutePayload MapToPayload(
        Route route)
    {
        return new RoutePayload
        {
            Id = route.Id,
            Name = route.Name,
            Description = route.Description,
            Path = route.Path,
            Type = route.Type,
            Attrs = route.Attrs,
            Providers = route.Providers
                .Select(x => new RouteProviderPayload
                {
                    Id = x.Id,
                    AppProviderId = x.AppProviderId,
                    Model = x.Model
                })
                .ToList(),
        };        
    }
}