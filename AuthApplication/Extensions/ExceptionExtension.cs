using AuthApplication.Middlewares;

namespace AuthApplication.Extensions;

/// <summary>
/// 异常处理中间件扩展
/// </summary>
public static class ExceptionExtension
{
    public static IApplicationBuilder UseGlobalException(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }
}