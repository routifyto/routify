using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.AppUsers;
using Routify.Api.Models.Common;
using Routify.Core.Extensions;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Enums;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/users")]
public class AppUsersController(
    DatabaseContext databaseContext)
    : BaseController
{
    [HttpGet(Name = "GetAppUsers")]
    public async Task<ActionResult<PaginatedOutput<AppUserOutput>>> GetAppUsersAsync(
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
            .AppUsers
            .Include(x => x.User)
            .Where(x => x.AppId == appId);

        if (!string.IsNullOrWhiteSpace(after))
            query = query.Where(x => x.Id.CompareTo(after) > 0);

        // Limit the number of items to fetch
        limit = Math.Max(1, Math.Min(limit, 100));
        var appUsers = await query
            .OrderBy(x => x.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);


        var appUserOutputs = new List<AppUserOutput>();
        foreach (var appUser in appUsers)
        {
            if (appUser.User is null)
                continue;

            appUserOutputs.Add(MapToOutput(appUser, appUser.User));
        }

        var hasNext = appUsers.Count == limit;
        var nextCursor = hasNext ? appUsers.Last().Id : null;
        var output = new PaginatedOutput<AppUserOutput>
        {
            Items = appUserOutputs,
            HasNext = hasNext,
            NextCursor = nextCursor,
        };

        return Ok(output);
    }

    [HttpPost(Name = "CreateAppUser")]
    public async Task<ActionResult<AppUsersOutput>> CreateAppUserAsync(
        [FromRoute] string appId,
        [FromBody] AppUsersInput input,
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

        if (input.Role is AppRole.Owner && currentAppUser.Role is not AppRole.Owner)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotAddOwner,
                Message = "You do not have access to add an owner"
            });
        }

        if (input.Role is AppRole.Admin && currentAppUser.Role is not AppRole.Owner)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotAddAdmin,
                Message = "You do not have access to add an admin"
            });
        }

        if (input.Role is AppRole.Member && currentAppUser.Role is not AppRole.Owner and AppRole.Admin)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotAddMember,
                Message = "You do not have access to add a member"
            });
        }

        if (input.Emails.Count == 0)
        {
            return BadRequest(new ApiErrorOutput
            {
                Code = ApiError.EmailListEmpty,
                Message = "You must include at least one email"
            });
        }
        
        if (input.Emails.Any(x => !IsValidEmail(x)))
        {
            return BadRequest(new ApiErrorOutput
            {
                Code = ApiError.InvalidEmail,
                Message = "Invalid email address"
            });
        }

        var users = await databaseContext
            .Users
            .Where(x => input.Emails.Contains(x.Email))
            .ToListAsync(cancellationToken);

        var existingAppUsers = await databaseContext
            .AppUsers
            .Where(x => x.AppId == appId)
            .ToListAsync(cancellationToken);

        var usersEmailDict = users.ToDictionary(x => x.Email);
        var emailsToCreate = input.Emails.Except(usersEmailDict.Keys).ToList();
        if (emailsToCreate.Count > 0)
        {
            var createdUsers = await CreateUsersByEmail(emailsToCreate);
            foreach (var user in createdUsers)
                usersEmailDict[user.Email] = user;
        }

        var appUsers = new List<AppUser>();
        foreach (var email in input.Emails)
        {
            if (!usersEmailDict.TryGetValue(email, out var user))
                continue;

            var existingAppUser = existingAppUsers.FirstOrDefault(x => x.UserId == user.Id);
            if (existingAppUser is not null)
            {
                existingAppUser.Role = input.Role;
                appUsers.Add(existingAppUser);
            }
            else
            {
                var newAppUser = new AppUser
                {
                    Id = RoutifyId.Generate(IdType.AppUser),
                    AppId = currentAppUser.AppId,
                    UserId = user.Id,
                    Role = input.Role,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = CurrentUserId,
                };
                appUsers.Add(newAppUser);
                databaseContext.AppUsers.Add(newAppUser);
            }
        }

        await databaseContext.SaveChangesAsync(cancellationToken);

        var appUserOutputs = new List<AppUserOutput>();
        var usersIdDict = users.ToDictionary(x => x.Id);
        foreach (var item in appUsers)
        {
            if (!usersIdDict.TryGetValue(item.UserId, out var user))
                continue;

            appUserOutputs.Add(MapToOutput(item, user));
        }

        var output = new AppUsersOutput
        {
            Users = appUserOutputs,
        };

        return Ok(output);
    }

    [HttpPut("{appUserId}", Name = "UpdateAppUser")]
    public async Task<ActionResult<AppUserOutput>> UpdateAppUserAsync(
        [FromRoute] string appId,
        [FromRoute] string appUserId,
        [FromBody] AppUserInput input,
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

        if (input.Role is AppRole.Owner && currentAppUser.Role is not AppRole.Owner)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotAddOwner,
                Message = "You do not have access to add an owner"
            });
        }

        if (input.Role is AppRole.Admin && currentAppUser.Role is not AppRole.Owner)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotAddAdmin,
                Message = "You do not have access to add an admin"
            });
        }
        
        if (input.Role is AppRole.Member && currentAppUser.Role is not AppRole.Owner and AppRole.Admin)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotAddMember,
                Message = "You do not have access to add a member"
            });
        }

        var appUserToUpdate = await databaseContext
            .AppUsers
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == appUserId, cancellationToken);

        if (appUserToUpdate is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.AppUserNotFound,
                Message = "User not found or does not belong to the app"
            });
        }

        if (appUserToUpdate.AppId != appId)
        {
            return BadRequest(new ApiErrorOutput
            {
                Code = ApiError.AppUserNotFound,
                Message = "User not found or does not belong to the app"
            });
        }

        var user = appUserToUpdate.User;
        if (user == null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.AppUserNotFound,
                Message = "User not found or does not belong to the app"
            });
        }

        if (appUserToUpdate.Role == input.Role)
        {
            return Ok(MapToOutput(appUserToUpdate, user));
        }

        if (appUserToUpdate.Role is AppRole.Owner)
        {
            var otherOwners = await databaseContext
                .AppUsers
                .CountAsync(x => x.AppId == appId && x.Id != appUserToUpdate.Id && x.Role == AppRole.Owner,
                    cancellationToken);
            
            if (otherOwners == 0)
            {
                return BadRequest(new ApiErrorOutput
                {
                    Code = ApiError.CannotRemoveLastOwner,
                    Message = "You cannot remove the last owner"
                });
            }
        }

        appUserToUpdate.Role = input.Role;
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = MapToOutput(appUserToUpdate, user);
        return Ok(output);
    }

    [HttpDelete("{appUserId}", Name = "DeleteAppUser")]
    public async Task<ActionResult<DeleteOutput>> DeleteAppUserAsync(
        [FromRoute] string appId,
        [FromRoute] string appUserId,
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

        var appUserToDelete = await databaseContext
            .AppUsers
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == appUserId, cancellationToken);

        if (appUserToDelete is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.AppUserNotFound,
                Message = "User not found or does not belong to the app"
            });
        }

        if (appUserToDelete.AppId != appId)
        {
            return BadRequest(new ApiErrorOutput
            {
                Code = ApiError.AppUserNotFound,
                Message = "User not found or does not belong to the app"
            });
        }

        var user = appUserToDelete.User;
        if (user is null)
        {
            return NotFound(new ApiErrorOutput
            {
                Code = ApiError.AppUserNotFound,
                Message = "User not found or does not belong to the app"
            });
        }
        
        if (appUserToDelete.Role is AppRole.Owner && currentAppUser.Role is not AppRole.Owner)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotRemoveOwner,
                Message = "You do not have access to remove the owner"
            });
        }
        
        if (appUserToDelete.Role is AppRole.Admin && currentAppUser.Role is not AppRole.Owner)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotRemoveAdmin,
                Message = "You do not have access to remove the admin"
            });
        }
        
        if (appUserToDelete.Role is AppRole.Member && currentAppUser.Role is not AppRole.Owner and AppRole.Admin)
        {
            return Forbidden(new ApiErrorOutput
            {
                Code = ApiError.CannotRemoveMember,
                Message = "You do not have access to remove the member"
            });
        }
        
        if (appUserToDelete.Role is AppRole.Owner)
        {
            var otherOwners = await databaseContext
                .AppUsers
                .CountAsync(x => x.AppId == appId && x.Id != appUserToDelete.Id && x.Role == AppRole.Owner,
                    cancellationToken);
            
            if (otherOwners == 0)
            {
                return BadRequest(new ApiErrorOutput
                {
                    Code = ApiError.CannotRemoveLastOwner,
                    Message = "You cannot remove the last owner"
                });
            }
        }

        databaseContext.AppUsers.Remove(appUserToDelete);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var output = new DeleteOutput
        {
            Id = appUserToDelete.Id
        };

        return Ok(output);
    }

    private async Task<List<User>> CreateUsersByEmail(
        IEnumerable<string> emails)
    {
        var users = new List<User>();
        foreach (var email in emails)
        {
            var user = new User
            {
                Id = RoutifyId.Generate(IdType.User),
                Name = email.GetNameFromEmail() ?? email,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                Status = UserStatus.Pending,
            };
            users.Add(user);
        }

        databaseContext.Users.AddRange(users);
        await databaseContext.SaveChangesAsync();
        return users;
    }

    private static AppUserOutput MapToOutput(
        AppUser appUser,
        User user)
    {
        return new AppUserOutput
        {
            Id = appUser.Id,
            Role = appUser.Role,
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email,
            Avatar = user.Avatar,
            CreatedAt = appUser.CreatedAt,
        };
    }

    private static bool IsValidEmail(
        string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}