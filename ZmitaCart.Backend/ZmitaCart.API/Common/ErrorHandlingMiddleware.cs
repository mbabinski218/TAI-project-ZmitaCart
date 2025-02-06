using System.Text.Json;
using FluentValidation;

namespace ZmitaCart.API.Common;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotImplementedException)
        {
            context.Response.StatusCode = StatusCodes.Status501NotImplemented;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse("Not implemented yet."));
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse(ex.Errors.Select(vf => vf.ErrorMessage)));
        }
        catch (UnauthorizedAccessException ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse(ex.Message));
        }
        catch (Exception ex) when (ex is ArgumentException or InvalidDataException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogCritical("{msg}", ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(CreateJsonResponse("An unexpected error occurred."));
        }
    }
    
    private static string CreateJsonResponse(IEnumerable<string> messages) => JsonSerializer.Serialize(messages);
    private static string CreateJsonResponse(string message) => JsonSerializer.Serialize(new List<string>{ new(message) });
}