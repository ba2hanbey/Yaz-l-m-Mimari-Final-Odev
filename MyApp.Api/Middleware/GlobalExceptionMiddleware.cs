using System.Net;
using System.Text.Json;
using MyApp.Application.Common;
using MyApp.Application.Exceptions;

namespace MyApp.Api.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");

            var (code, message) = ex switch
            {
                ValidationException ve => (HttpStatusCode.BadRequest, ve.Message),
                NotFoundException nf => (HttpStatusCode.NotFound, nf.Message),
                ConflictException cf => (HttpStatusCode.Conflict, cf.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            var body = JsonSerializer.Serialize(ApiResponse<object>.Fail(message));
            await context.Response.WriteAsync(body);
        }
    }
}
