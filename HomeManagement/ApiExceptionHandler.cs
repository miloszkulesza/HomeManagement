using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HomeManagement.Core.Exceptions;

namespace HomeManagement;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ArgumentException => (StatusCodes.Status400BadRequest, "Nieprawidłowe dane"),
            KeyNotFoundException => (StatusCodes.Status404NotFound, "Nie znaleziono zasobu"),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Brak autoryzacji"),
            ConflictException => (StatusCodes.Status409Conflict, "Nie można wykonać operacji"),
            _ => (StatusCodes.Status500InternalServerError, "Wystąpił błąd serwera")
        };

        if (statusCode >= StatusCodes.Status500InternalServerError)
            logger.LogError(exception, "Nieobsłużony wyjątek podczas realizacji żądania.");
        else
            logger.LogWarning(exception, "Żądanie zakończyło się błędem {StatusCode}.", statusCode);

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode < StatusCodes.Status500InternalServerError ? exception.Message : null,
            Instance = httpContext.Request.Path
        }, cancellationToken);

        return true;
    }
}
