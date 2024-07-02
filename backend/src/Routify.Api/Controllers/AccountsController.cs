using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Routify.Api.Models.Accounts;
using Routify.Api.Models.Errors;
using Routify.Core.Utils;
using Routify.Data;
using Routify.Data.Models;

namespace Routify.Api.Controllers;

[Route("v1/accounts")]
public class AccountsController(
    DatabaseContext databaseContext,
    IConfiguration configuration,
    IPasswordHasher<User> passwordHasher) 
    : BaseController
{
    private const string GoogleUserInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo";
    
    private string JwtSecret => configuration["Jwt:Secret"] ?? throw new InvalidOperationException("Jwt:Secret is missing.");
    private string JwtIssuer => configuration["Jwt:Issuer"] ?? "routify";
    private string JwtAudience => configuration["Jwt:Audience"] ?? "routify";
    
    [HttpPost("register/email", Name = "RegisterWithEmail")]
    public async Task<ActionResult> RegisterAsync(
        [FromBody] EmailRegisterInput input,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await databaseContext
            .Users
            .SingleOrDefaultAsync(x => x.Email == input.Email, cancellationToken);

        if (existingUser is not null)
        {
            return BadRequest(new ApiErrorPayload
            {
                Code = ApiError.EmailAlreadyExists,
                Message = "Email already exists."
            });
        }
        
        var user = new User
        {
            Id = RoutifyId.Generate(IdType.User),
            Name = input.Name,
            Email = input.Email,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow
        };
        
        user.Password = passwordHasher.HashPassword(user, input.Password);
        
        await databaseContext
            .Users
            .AddAsync(user, cancellationToken);
        
        await databaseContext.SaveChangesAsync(cancellationToken);
        return Ok(BuildLoginPayload(user));
    }
    
    [HttpPost("login/email", Name = "LoginWithEmail")]
    public async Task<ActionResult> LoginWithFacebookAsync(
        [FromBody] EmailLoginInput input,
        CancellationToken cancellationToken = default)
    {
        var user = await databaseContext
            .Users
            .SingleOrDefaultAsync(x => x.Email == input.Email, cancellationToken);

        if (user is null)
        {
            return BadRequest(new ApiErrorPayload
            {
                Code = ApiError.InvalidEmailOrPassword,
                Message = "Invalid email or password."
            });
        }

        if (string.IsNullOrWhiteSpace(user.Password))
        {
            return BadRequest(new ApiErrorPayload
            {
                Code = ApiError.InvalidEmailOrPassword,
                Message = "Invalid email or password."
            });
        }
        
        var result = passwordHasher.VerifyHashedPassword(user, user.Password, input.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            return BadRequest(new ApiErrorPayload
            {
                Code = ApiError.InvalidEmailOrPassword,
                Message = "Invalid email or password."
            });
        }

        if (result == PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.Password = passwordHasher.HashPassword(user, input.Password);
            user.UpdatedAt = DateTime.UtcNow;
            await databaseContext.SaveChangesAsync(cancellationToken);
        }
        
        return Ok(BuildLoginPayload(user));
    }

    [HttpPost("login/google", Name = "LoginWithGoogle")]
    public async Task<ActionResult<LoginPayload>> LoginWithGoogleAsync(
        [FromBody] GoogleLoginInput input,
        CancellationToken cancellationToken = default)
    {
        using var client = new HttpClient();
        var url = $"{GoogleUserInfoUrl}?access_token={input.AccessToken}";
        var userInfoResponse = await client.GetAsync(url, cancellationToken);

        if (!userInfoResponse.IsSuccessStatusCode)
        {
            return BadRequest(new ApiErrorPayload
            {
                Code = ApiError.GoogleAuthFailed,
                Message = "Failed to authenticate with Google."
            });
        }

        var responseContent = await userInfoResponse.Content.ReadAsStringAsync(cancellationToken);
        var googleUser = JsonSerializer.Deserialize<GoogleUserInfo>(responseContent);

        if (googleUser is null)
        {
            return BadRequest(new ApiErrorPayload
            {
                Code = ApiError.GoogleAuthFailed,
                Message = "Failed to authenticate with Google."
            });
        }
        
        var existingUser = await databaseContext
            .Users
            .SingleOrDefaultAsync(x => x.Email == googleUser.Email, cancellationToken);

        if (existingUser is not null)
        {
            if (existingUser.Status == UserStatus.Pending)
            {
                return BadRequest(new ApiErrorPayload
                {
                    Code = ApiError.UserPendingActivation,
                    Message = "User is pending activation."
                });
            }
            
            if (existingUser.Attrs == null || string.IsNullOrWhiteSpace(existingUser.Attrs.GoogleId))
            {
                existingUser.Attrs = new UserAttrs
                {
                    GoogleId = googleUser.Id
                };
                existingUser.UpdatedAt = DateTime.UtcNow;
                await databaseContext.SaveChangesAsync(cancellationToken);
            }
            
            return Ok(BuildLoginPayload(existingUser));
        }
            
        var user = new User
        {
            Id = RoutifyId.Generate(IdType.User),
            Name = googleUser.Name,
            Email = googleUser.Email,
            Status = UserStatus.Active,
            CreatedAt = DateTime.UtcNow,
            Attrs = new UserAttrs
            {
                GoogleId = googleUser.Id
            }
        };
        
        await databaseContext
            .Users
            .AddAsync(user, cancellationToken);
        
        await databaseContext.SaveChangesAsync(cancellationToken);
        return BuildLoginPayload(user);
    }

    private LoginPayload BuildLoginPayload(
        User user)
    {
        var keyBytes = Encoding.UTF8.GetBytes(JwtSecret);
        var key = new SymmetricSecurityKey(keyBytes);
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = JwtIssuer,
            Audience = JwtAudience,
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("sub", user.Id),
                new Claim("name", user.Name),
                new Claim("email", user.Email)
            }),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMonths(1),
            SigningCredentials = credentials
        };
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(tokenDescriptor);
        var tokenString = handler.WriteToken(token);

        return new LoginPayload
        {
            Token = tokenString
        };
    }
}