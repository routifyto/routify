using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.ApiKeys;
using Routify.Api.Models.Common;
using Routify.Core.Extensions;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Enums;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/api-keys")]
public class ApiKeysController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet(Name = "GetApiKeys")]
    public async Task<ActionResult<PaginatedOutput<ApiKeyOutput>>> GetApiKeysAsync(
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
            .ApiKeys
            .Where(x => x.AppId == appId);

        if (!string.IsNullOrWhiteSpace(after))
            query = query.Where(x => x.Id.CompareTo(after) > 0);

        // Limit the number of items to fetch
        limit = Math.Max(1, Math.Min(limit, 100));
        var apiKeys = await query
            .OrderBy(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var apiKeyOutputs = new List<ApiKeyOutput>();
        foreach (var apiKey in apiKeys)
        {
            var output = new ApiKeyOutput
            {
                Id = apiKey.Id,
                Name = apiKey.Name,
                Description = apiKey.Description,
                CanUseGateway = apiKey.CanUseGateway,
                Role = apiKey.Role,
                Prefix = apiKey.Prefix,
                Suffix = apiKey.Suffix,
                ExpiresAt = apiKey.ExpiresAt,
                CostLimitConfig = apiKey.CostLimitConfig
            };

            apiKeyOutputs.Add(output);
        }

        var hasNext = apiKeys.Count == limit;
        var nextCursor = hasNext ? apiKeys.Last().Id : null;
        return Ok(new PaginatedOutput<ApiKeyOutput>
        {
            Items = apiKeyOutputs,
            HasNext = hasNext,
            NextCursor = nextCursor
        });
    }

    [HttpGet("{apiKeyId}", Name = "GetApiKey")]
    public async Task<ActionResult<ApiKeyOutput>> GetApiKeyAsync(
        [FromRoute] string appId,
        [FromRoute] string apiKeyId,
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
        
        var apiKey = await databaseContext
            .ApiKeys
            .SingleOrDefaultAsync(x => x.Id == apiKeyId, cancellationToken);

        if (apiKey is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.ApiKeyNotFound,
                Message = "API key was not found or has been deleted"
            });
        }

        var output = new ApiKeyOutput
        {
            Id = apiKey.Id,
            Name = apiKey.Name,
            Description = apiKey.Description,
            CanUseGateway = apiKey.CanUseGateway,
            Role = apiKey.Role,
            Prefix = apiKey.Prefix,
            Suffix = apiKey.Suffix,
            ExpiresAt = apiKey.ExpiresAt,
            CostLimitConfig = apiKey.CostLimitConfig
        };

        return Ok(output);
    }

    [HttpPost(Name = "CreateApiKey")]
    public async Task<ActionResult<CreateApiKeyOutput>> CreateApiKeyAsync(
        [FromRoute] string appId,
        [FromBody] ApiKeyInput input,
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
        
        if (!CanManageApiKeys(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageApiKeys,
                Message = "You do not have permission to manage API keys"
            });
        }

        var id = RoutifyId.Generate(IdType.ApiKey);
        var salt = Guid.NewGuid().ToString("N");
        var secret = $"{Guid.NewGuid():N}{Guid.NewGuid():N}";
        var hash = $"{secret}{salt}".ToSha256();
        var suffix = secret[^4..];
        var key = $"rtf_{id}{secret}";

        var apiKey = new ApiKey
        {
            Id = id,
            AppId = appId,
            Name = input.Name,
            Description = input.Description,
            CanUseGateway = input.CanUseGateway,
            Role = input.Role,
            Prefix = "rtf",
            Hash = hash,
            Salt = salt,
            Suffix = suffix,
            Algorithm = ApiKeyHashAlgorithm.Sha256,
            ExpiresAt = input.ExpiresAt,
            CostLimitConfig = input.CostLimitConfig,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = CurrentUserId,
            VersionId = RoutifyId.Generate(IdType.Version),
            Status = ApiKeyStatus.Active
        };

        databaseContext.ApiKeys.Add(apiKey);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = new CreateApiKeyOutput
        {
            Key = key,
            ApiKey = new ApiKeyOutput
            {
                Id = apiKey.Id,
                Name = apiKey.Name,
                Description = apiKey.Description,
                CanUseGateway = apiKey.CanUseGateway,
                Role = apiKey.Role,
                Prefix = apiKey.Prefix,
                Suffix = apiKey.Suffix,
                ExpiresAt = apiKey.ExpiresAt,
                CostLimitConfig = apiKey.CostLimitConfig
            }
        };

        return Ok(output);
    }

    [HttpPut("{apiKeyId}", Name = "UpdateApiKey")]
    public async Task<ActionResult<ApiKeyOutput>> UpdateApiKeyAsync(
        [FromRoute] string appId,
        [FromRoute] string apiKeyId,
        [FromBody] ApiKeyInput input,
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
        
        if (!CanManageApiKeys(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageApiKeys,
                Message = "You do not have permission to manage API keys"
            });
        }

        var apiKey = await databaseContext
            .ApiKeys
            .SingleOrDefaultAsync(x => x.Id == apiKeyId, cancellationToken);

        if (apiKey is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.ApiKeyNotFound,
                Message = "API key was not found or has been deleted"
            });
        }

        apiKey.Name = input.Name;
        apiKey.Description = input.Description;
        apiKey.CanUseGateway = input.CanUseGateway;
        apiKey.Role = input.Role;
        apiKey.ExpiresAt = input.ExpiresAt;
        apiKey.CostLimitConfig = input.CostLimitConfig;
        apiKey.UpdatedAt = DateTime.UtcNow;
        apiKey.UpdatedBy = CurrentUserId;
        apiKey.VersionId = RoutifyId.Generate(IdType.Version);

        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = new ApiKeyOutput
        {
            Id = apiKey.Id,
            Name = apiKey.Name,
            Description = apiKey.Description,
            CanUseGateway = apiKey.CanUseGateway,
            Role = apiKey.Role,
            Prefix = apiKey.Prefix,
            Suffix = apiKey.Suffix,
            ExpiresAt = apiKey.ExpiresAt,
            CostLimitConfig = apiKey.CostLimitConfig
        };

        return Ok(output);
    }

    [HttpDelete("{apiKeyId}", Name = "DeleteApiKey")]
    public async Task<ActionResult<DeleteOutput>> DeleteApiKeyAsync(
        [FromRoute] string appId,
        [FromRoute] string apiKeyId,
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
        
        if (!CanManageApiKeys(currentAppUser))
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotManageApiKeys,
                Message = "You do not have permission to manage API keys"
            });
        }
        
        var apiKey = await databaseContext
            .ApiKeys
            .SingleOrDefaultAsync(x => x.Id == apiKeyId, cancellationToken);

        if (apiKey is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.ApiKeyNotFound,
                Message = "API key was not found or has been deleted"
            });
        }

        databaseContext.ApiKeys.Remove(apiKey);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = new DeleteOutput
        {
            Id = apiKey.Id
        };

        return Ok(output);
    }
    
    private static bool CanManageApiKeys(
        AppUser appUser)
    {
        return appUser.Role is AppRole.Owner or AppRole.Admin;
    }
}