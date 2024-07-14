using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.Common;
using Routify.Api.Models.Consumers;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Enums;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/consumers")]
public class ConsumersController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet(Name = "GetConsumers")]
    public async Task<ActionResult<PaginatedOutput<ConsumerOutput>>> GetConsumersAsync(
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
            .Consumers
            .Where(x => x.AppId == appId);

        if (!string.IsNullOrWhiteSpace(after))
            query = query.Where(x => x.Id.CompareTo(after) > 0);

        // Limit the number of items to fetch
        limit = Math.Max(1, Math.Min(limit, 100));
        var consumers = await query
            .OrderBy(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var consumerOutputs = consumers
            .Select(MapToOutput)
            .ToList();
        
        var hasNext = consumers.Count == limit;
        var nextCursor = hasNext ? consumers.Last().Id : null;
        var output = new PaginatedOutput<ConsumerOutput>
        {
            Items = consumerOutputs,
            HasNext = hasNext,
            NextCursor = nextCursor
        };
        
        return Ok(output);
    }
    
    [HttpGet("{consumerId}", Name = "GetConsumer")]
    public async Task<ActionResult<ConsumerOutput>> GetConsumerAsync(
        [FromRoute] string appId,
        [FromRoute] string consumerId,
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

        var consumer = await databaseContext
            .Consumers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == consumerId, cancellationToken);

        if (consumer is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.ConsumerNotFound,
                Message = "Consumer was not found or has been deleted"
            });
        }

        var output = MapToOutput(consumer);
        return Ok(output);
    }
    
    [HttpPost(Name = "CreateConsumer")]
    public async Task<ActionResult<ConsumerOutput>> CreateConsumerAsync(
        [FromRoute] string appId,
        [FromBody] ConsumerInput input,
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
        
        if (!CanManageConsumer(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageConsumers,
                Message = "You do not have permission to manage consumers"
            });
        }

        var consumer = new Consumer
        {
            Id = RoutifyId.Generate(IdType.Consumer),
            AppId = appId,
            Name = input.Name,
            Description = input.Description,
            Alias = input.Alias,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = CurrentUserId,
            VersionId = RoutifyId.Generate(IdType.Version),
            Status = ConsumerStatus.Active
        };

        databaseContext.Consumers.Add(consumer);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = MapToOutput(consumer);
        return Ok(output);
    }
    
    [HttpPut("{consumerId}", Name = "UpdateConsumer")]
    public async Task<ActionResult<ConsumerOutput>> UpdateConsumerAsync(
        [FromRoute] string appId,
        [FromRoute] string consumerId,
        [FromBody] ConsumerInput input,
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

        var consumer = await databaseContext
            .Consumers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == consumerId, cancellationToken);

        if (consumer is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.ConsumerNotFound,
                Message = "Consumer was not found or has been deleted"
            });
        }
        
        if (!CanManageConsumer(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageConsumers,
                Message = "You do not have permission to manage consumers"
            });
        }

        consumer.Name = input.Name;
        consumer.Description = input.Description;
        consumer.Alias = input.Alias;
        consumer.UpdatedAt = DateTime.UtcNow;
        consumer.UpdatedBy = CurrentUserId;
        consumer.VersionId = RoutifyId.Generate(IdType.Version);

        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = MapToOutput(consumer);
        return Ok(output);
    }
    
    [HttpDelete("{consumerId}", Name = "DeleteConsumer")]
    public async Task<ActionResult> DeleteConsumerAsync(
        [FromRoute] string appId,
        [FromRoute] string consumerId,
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

        var consumer = await databaseContext
            .Consumers
            .SingleOrDefaultAsync(x => x.AppId == appId && x.Id == consumerId, cancellationToken);

        if (consumer is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.ConsumerNotFound,
                Message = "Consumer was not found or has been deleted"
            });
        }
        
        if (!CanManageConsumer(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageConsumers,
                Message = "You do not have permission to manage consumers"
            });
        }

        databaseContext.Consumers.Remove(consumer);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = new DeleteOutput
        {
            Id = consumer.Id
        };
        
        return Ok(output);
    }
    
    private static ConsumerOutput MapToOutput(
        Consumer consumer)
    {
        return new ConsumerOutput
        {
            Id = consumer.Id,
            Name = consumer.Name,
            Description = consumer.Description,
            Alias = consumer.Alias
        };
    }
    
    private static bool CanManageConsumer(
        AppUser appUser)
    {
        return appUser.Role is AppRole.Owner or AppRole.Admin;
    }
}