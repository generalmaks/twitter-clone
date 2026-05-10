using System.Net;
using System.Text.Json;

namespace TwitterCloneBack;

public class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ArgumentException ex)
        {
            logger.LogError(ex, "Argument Exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode =
                (int)HttpStatusCode.BadRequest;

            var response =
                new
                {
                    statusCode = context.Response.StatusCode,
                    message = $"{ex.Message}"
                };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Not Found Exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode =
                (int)HttpStatusCode.NotFound;

            var response =
                new
                {
                    statusCode = context.Response.StatusCode,
                    message = $"{ex.Message}"
                };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            var response =
                new
                {
                    statusCode = context.Response.StatusCode,
                    message = $"{ex.Message}"
                };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}