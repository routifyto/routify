using Microsoft.AspNetCore.Mvc;
using Routify.Api.Models.Common;

namespace Routify.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
    protected string CurrentUserId => User.FindFirst("sub")?.Value ?? "null";
    
    protected ObjectResult Forbidden(
        ApiErrorOutput output)
    {
        return StatusCode(403, output);
    }
}