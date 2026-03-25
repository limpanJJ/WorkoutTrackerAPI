using Microsoft.AspNetCore.Mvc;
using WorkoutTrackerAPI.Exceptions;

namespace WorkoutTrackerAPI.Infrastructure;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title) = exception switch
        {
            UnauthorizedException       => (StatusCodes.Status401Unauthorized, "Unauthorized"),
            ConflictException           => (StatusCodes.Status409Conflict, "Conflict"),
            NotFoundException           => (StatusCodes.Status404NotFound, "Not Found"),
            _                           => (StatusCodes.Status500InternalServerError, "Internal Server Error"),
        };

        // Always log, but only at Error level for unexpected (5xx) failures
        if (statusCode >= 500)
            logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);
        else
            logger.LogWarning(exception, "Handled exception ({StatusCode}): {Message}", statusCode, exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment()
                ? exception.Message
                : statusCode >= 500
                    ? "An unexpected error occurred."
                    : exception.Message,
        };

        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}