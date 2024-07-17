using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Common;
using Routify.Api.Models.Routes;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Common;
using Routify.Data.Enums;
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
        {
            return Unauthorized(new ApiErrorOutput
            {
                Code = ApiError.Unauthorized,
                Message = "Unauthorized access"
            });
        }

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }

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
        {
            return Unauthorized(new ApiErrorOutput
            {
                Code = ApiError.Unauthorized,
                Message = "Unauthorized access"
            });
        }

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }

        var route = await databaseContext
            .Routes
            .Include(x => x.Providers)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);

        if (route is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.RouteNotFound,
                Message = "Route was not found or has been deleted"
            });
        }
        
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
        {
            return Unauthorized(new ApiErrorOutput
            {
                Code = ApiError.Unauthorized,
                Message = "Unauthorized access"
            });
        }

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }

        if (!CanManageRoutes(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageRoutes,
                Message = "You do not have permission to manage routes"
            });
        }

        var route = new Route
        {
            Id = RoutifyId.Generate(IdType.Route),
            AppId = appId,
            Name = input.Name,
            Description = input.Description,
            Path = input.Path,
            Type = input.Type,
            Strategy = input.Strategy,
            Schema = input.Schema,
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
                Weight = x.Weight,
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
        {
            return Unauthorized(new ApiErrorOutput
            {
                Code = ApiError.Unauthorized,
                Message = "Unauthorized access"
            });
        }

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }
        
        if (!CanManageRoutes(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageRoutes,
                Message = "You do not have permission to manage routes"
            });
        }

        var route = await databaseContext
            .Routes
            .Include(x => x.Providers)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);

        if (route is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.RouteNotFound,
                Message = "Route not found"
            });
        }

        route.Name = input.Name;
        route.Description = input.Description;
        route.Path = input.Path;
        route.Strategy = input.Strategy;
        route.Schema = input.Schema;
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
                    Weight = routeProviderInput.Weight,
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
                routeProvider.Weight = routeProviderInput.Weight;
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
        {
            return Unauthorized(new ApiErrorOutput
            {
                Code = ApiError.Unauthorized,
                Message = "Unauthorized access"
            });
        }

        var currentAppUser = await databaseContext
            .AppUsers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);

        if (currentAppUser is null)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.NoAppAccess,
                Message = "You do not have access to the app"
            });
        }
        
        if (!CanManageRoutes(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageRoutes,
                Message = "You do not have permission to manage routes"
            });
        }

        var route = await databaseContext
            .Routes
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == routeId, cancellationToken);

        if (route is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.RouteNotFound,
                Message = "Route not found"
            });
        }

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
            Strategy = route.Strategy,
            Schema = route.Schema,
            Providers = route.Providers
                .Select(x => new RouteProviderOutput
                {
                    Id = x.Id,
                    AppProviderId = x.AppProviderId,
                    Model = x.Model,
                    Attrs = x.Attrs,
                    Weight = x.Weight
                })
                .ToList(),
        };
    }

    private static bool CanManageRoutes(
        AppUser appUser)
    {
        return appUser.Role is AppRole.Owner or AppRole.Admin;
    }
}