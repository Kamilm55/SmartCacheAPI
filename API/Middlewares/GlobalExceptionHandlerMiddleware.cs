using System.Net;
using System.Text.Json;
using SmartCacheManagementSystem.Common;
using SmartCacheManagementSystem.Common.Exceptions;

namespace SmartCacheManagementSystem.API.Middlewares;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception caught in global handler.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        // Default values
        var statusCode = (int)HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred.";
        
        if (exception is KeyNotFoundException) { statusCode = 404; message = exception.Message; }
        if (exception is NotFoundException) { statusCode = 404; message = exception.Message; }

        if(exception is ArgumentNullException || exception is InvalidOperationException){ statusCode = 400; message = exception.Message; }
        
        var response = new ApiResponse<object>
        {
            IsSuccess = false,
            Message = message
           // Errors = new List<string> { exception.Message }
        };

        context.Response.StatusCode = statusCode;

        Console.WriteLine(exception.Message);
        Console.WriteLine(response.ToString());
        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        await context.Response.WriteAsync(json);
    }
}