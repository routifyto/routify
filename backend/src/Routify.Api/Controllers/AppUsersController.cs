using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Routify.Api.Models.AppUsers;
using Routify.Api.Models.Common;
using Routify.Core.Extensions;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/apps/{appId}/users")]
public class AppUsersController(
    DatabaseContext databaseContext) 
    : BaseController
{
    [HttpPost(Name = "CreateAppUser")]
    public async Task<ActionResult<AppUsersPayload>> CreateAppUserAsync(
        [FromRoute] string appId,
        [FromBody] AppUsersInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var appUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (appUser is null || appUser.Role != AppUserRole.Owner)
            return Forbid();
        
        var app = appUser.App;
        if (app is null)
            return NotFound();
        
        if (input.Emails.Count == 0)
            return BadRequest("Emails cannot be empty");
        
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
                    AppId = app.Id,
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
        
        var appUserPayloads = new List<AppUserPayload>();
        var usersIdDict = users.ToDictionary(x => x.Id);
        foreach (var item in appUsers)
        {
            if (!usersIdDict.TryGetValue(item.UserId, out var user))
                continue;
            
            appUserPayloads.Add(new AppUserPayload
            {
                Id = item.Id,
                Role = item.Role,
                UserId = item.UserId,
                Name = user.Name,
                Email = user.Email,
                Avatar = user.Avatar,
            });
        }
        
        var payload = new AppUsersPayload
        {
            Users = appUserPayloads,
        };
        
        return Ok(payload);
    }
    
    [HttpPut("{appUserId}", Name = "UpdateAppUser")]
    public async Task<ActionResult<AppUserPayload>> UpdateAppUserAsync(
        [FromRoute] string appId,
        [FromRoute] string appUserId,
        [FromBody] AppUserInput input,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var appUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (appUser is null || appUser.Role != AppUserRole.Owner)
            return Forbid();
        
        var app = appUser.App;
        if (app is null)
            return NotFound();
        
        var appUserToUpdate = await databaseContext
            .AppUsers
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == appUserId, cancellationToken);
        
        if (appUserToUpdate is null)
            return NotFound();
        
        if (appUserToUpdate.AppId != appId)
            return BadRequest("AppUser does not belong to the App");
        
        var user = appUserToUpdate.User;
        if (user == null)
            return NotFound();
        
        appUserToUpdate.Role = input.Role;
        await databaseContext.SaveChangesAsync(cancellationToken);
        
        var payload = new AppUserPayload
        {
            Id = appUserToUpdate.Id,
            Role = appUserToUpdate.Role,
            UserId = appUserToUpdate.UserId,
            Name = user.Name,
            Email = user.Email,
            Avatar = user.Avatar,
            CreatedAt = appUserToUpdate.CreatedAt,
        };
        
        return Ok(payload);
    }
    
    [HttpDelete("{appUserId}", Name = "DeleteAppUser")]
    public async Task<ActionResult<DeletePayload>> DeleteAppUserAsync(
        [FromRoute] string appId,
        [FromRoute] string appUserId,
        CancellationToken cancellationToken = default)
    {
        if (!IsAuthenticated)
            return Unauthorized();

        var appUser = await databaseContext
            .AppUsers
            .Include(x => x.App)
            .SingleOrDefaultAsync(x => x.AppId == appId && x.UserId == CurrentUserId, cancellationToken);
        
        if (appUser is null || appUser.Role != AppUserRole.Owner)
            return Forbid();
        
        var app = appUser.App;
        if (app is null)
            return NotFound();
        
        var appUserToDelete = await databaseContext
            .AppUsers
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.Id == appUserId, cancellationToken);
        
        if (appUserToDelete is null)
            return NotFound();
        
        if (appUserToDelete.AppId != appId)
            return BadRequest("AppUser does not belong to the App");
        
        var user = appUserToDelete.User;
        if (user is null)
            return NotFound();
        
        databaseContext.AppUsers.Remove(appUserToDelete);
        await databaseContext.SaveChangesAsync(cancellationToken);

        var payload = new DeletePayload
        {
            Id = appUserToDelete.Id
        };
        
        return Ok(payload);
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
}