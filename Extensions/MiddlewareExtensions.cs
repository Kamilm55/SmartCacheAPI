using SmartCacheManagementSystem.API.Middlewares;

namespace SmartCacheManagementSystem.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalMiddlewares(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        return app;
    }
}