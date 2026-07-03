using System.Text.Json;
using AuthApplication.Common;
using Domain.Exceptions;

namespace AuthApplication.Middlewares;

/// <summary>
/// 全局异常处理中间件
/// </summary>
public sealed class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BusinessException ex)
        {
            _logger.LogWarning(ex, ex.Message);

            await WriteResponseAsync(
                context,
                ApiResult.Fail(ex.Code, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            await WriteResponseAsync(
                context,
                ApiResult.Fail(ApiCode.Fail, ex.Message));
        }
    }

    private static async Task WriteResponseAsync(
        HttpContext context,
        ApiResult result)
    {
        context.Response.StatusCode = StatusCodes.Status200OK;
        context.Response.ContentType = "application/json;charset=utf-8";

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(result));
    }
}