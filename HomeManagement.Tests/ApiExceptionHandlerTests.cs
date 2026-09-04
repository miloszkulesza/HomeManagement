using System.Text.Json;
using FluentAssertions;
using HomeManagement.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;

namespace HomeManagement.Tests;

public class ApiExceptionHandlerTests
{
    [Fact]
    public async Task Unexpected_exception_returns_500_without_internal_details()
    {
        var context = CreateContext();
        var handler = new ApiExceptionHandler(NullLogger<ApiExceptionHandler>.Instance);

        await handler.TryHandleAsync(
            context,
            new InvalidOperationException("sensitive database implementation detail"),
            CancellationToken.None);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        var response = await ReadResponse(context);
        response.GetProperty("title").GetString().Should().Be("Wystąpił błąd serwera");
        response.TryGetProperty("detail", out var detail).Should().BeFalse();
    }

    [Fact]
    public async Task Domain_conflict_returns_409_with_safe_message()
    {
        var context = CreateContext();
        var handler = new ApiExceptionHandler(NullLogger<ApiExceptionHandler>.Instance);

        await handler.TryHandleAsync(
            context,
            new ConflictException("Konflikt danych użytkownika."),
            CancellationToken.None);

        context.Response.StatusCode.Should().Be(StatusCodes.Status409Conflict);
        var response = await ReadResponse(context);
        response.GetProperty("detail").GetString().Should().Be("Konflikt danych użytkownika.");
    }

    private static DefaultHttpContext CreateContext()
    {
        var context = new DefaultHttpContext();
        context.Request.Path = "/api/test";
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<JsonElement> ReadResponse(HttpContext context)
    {
        context.Response.Body.Position = 0;
        return await JsonSerializer.DeserializeAsync<JsonElement>(context.Response.Body);
    }
}
