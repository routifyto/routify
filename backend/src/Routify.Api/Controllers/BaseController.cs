using Microsoft.AspNetCore.Mvc;

namespace Routify.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected bool IsAuthenticated => User.Identity?.IsAuthenticated ?? false;
    protected string CurrentUserId => User.FindFirst("sub")?.Value ?? "null";
}