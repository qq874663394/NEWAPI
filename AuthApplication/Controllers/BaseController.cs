using AuthApplication.Common;
using Infrastructure.Authentication;
using Infrastructure.Authentication.CurrentUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApplication.Controllers;

/// <summary>
/// 控制器基类
/// </summary>
[ApiController]
[Authorize]
public abstract class BaseController : ControllerBase
{
    protected readonly ICurrentUser CurrentUser;

    protected BaseController(ICurrentUser currentUser)
    {
        CurrentUser = currentUser;
    }

    /// <summary>
    /// 当前用户Code
    /// </summary>
    protected string? Code => CurrentUser.Code;

    /// <summary>
    /// 当前用户名
    /// </summary>
    protected string? Name => CurrentUser.Name;

    /// <summary>
    /// 当前姓名
    /// </summary>
    protected string? FullName => CurrentUser.FullName;

    protected IActionResult Success()
    {
        return Ok(ApiResult.Ok());
    }

    protected IActionResult Success(object? data)
    {
        return Ok(ApiResult.Ok(data));
    }

    protected IActionResult Success(string message, object? data = null)
    {
        return Ok(ApiResult.Ok(message, data));
    }

    protected IActionResult Fail(string message)
    {
        return Ok(ApiResult.Fail(message));
    }

    protected IActionResult Fail(int code, string message)
    {
        return Ok(ApiResult.Fail(code, message));
    }
}