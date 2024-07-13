using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Common;
using Routify.Api.Models.Routes;
using Routify.Core.Constants;
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
    public async Task<ActionResult<PaginatedOutput<RouteOutput>>> GetRoutesAsync(
        [FromRoute] string appId,
        [FromQuery] string? after,
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
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

        var routeOutputs = routes
            .Select(MapToOutput)
            .ToList();

        var hasNext = routes.Count == limit;
        var nextCursor = hasNext ? routes.Last().Id : null;
        var output = new PaginatedOutput<RouteOutput>
        {
            Items = routeOutputs,
            HasNext = hasNext,
            NextCursor = nextCursor
        };

        return Ok(output);
    }
    
    [HttpGet("{routeId}", Name = "GetRoute")]
    public async Task<ActionResult<RouteOutput>> GetRouteAsync(
        [FromRoute] string appId,
        [FromRoute] string routeId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var route = await databaseContext
            .Routes
            .Include(x => x.Providers)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);
        
        if (route is null)
            return NotFound();
        
        var output = MapToOutput(route);
        return Ok(output);
    }
    
    [HttpPost(Name = "CreateRoute")]
    public async Task<ActionResult<RouteOutput>> CreateRouteAsync(
        [FromRoute] string appId,
        [FromBody] CreateRouteInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var route = new Route
        {
            Id = RoutifyId.Generate(IdType.Route),
            AppId = appId,
            Name = input.Name,
            Description = input.Description,
            Path = input.Path,
            Type = input.Type,
            Schema = ProviderIds.OpenAi,
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
                Attrs = x.Attrs ?? new Dictionary<string, string>(),
                CreatedAt = DateTime.UtcNow,
                CreatedBy = CurrentUserId,
                VersionId = RoutifyId.Generate(IdType.Version)
            })
            .ToList();

        databaseContext.Routes.Add(route);
        databaseContext.RouteProviders.AddRange(routeProviders);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var routeOutput = MapToOutput(route);
        return CreatedAtRoute("GetRoute", new { appId, routeId = route.Id }, routeOutput);
    }
    
    [HttpPut("{routeId}", Name = "UpdateRoute")]
    public async Task<ActionResult<RouteOutput>> UpdateRouteAsync(
        [FromRoute] string appId,
        [FromRoute] string routeId,
        [FromBody] UpdateRouteInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
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
                    Attrs = routeProviderInput.Attrs ?? new Dictionary<string, string>(),
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
                routeProvider.Attrs = routeProviderInput.Attrs ?? new Dictionary<string, string>();
                routeProvider.UpdatedAt = DateTime.UtcNow;
                routeProvider.UpdatedBy = CurrentUserId;
                routeProvider.VersionId = RoutifyId.Generate(IdType.Version);
            }
        }
        
        await databaseContext.SaveChangesAsync(cancellationToken);

        var routeOutput = MapToOutput(route);
        return Ok(routeOutput);
    }
    
    [HttpDelete("{routeId}", Name = "DeleteRoute")]
    public async Task<ActionResult<DeleteOutput>> DeleteRouteAsync(
        [FromRoute] string appId,
        [FromRoute] string routeId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
            return NotFound();

        var route = await databaseContext
            .Routes
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);

        if (route is null)
            return NotFound();

        databaseContext.Routes.Remove(route);
        await databaseContext.SaveChangesAsync(cancellationToken);

        return Ok(new DeleteOutput());
    }

    private static RouteOutput MapToOutput(
        Route route)
    {
        return new RouteOutput
        {
            Id = route.Id,
            Name = route.Name,
            Description = route.Description,
            Path = route.Path,
            Type = route.Type,
            Attrs = route.Attrs,
            Providers = route.Providers
                .Select(x => new RouteProviderOutput
                {
                    Id = x.Id,
                    AppProviderId = x.AppProviderId,
                    Model = x.Model,
                    Attrs = x.Attrs
                })
                .ToList(),
        };        
    }
}