using System.Net;
using System.Text.Json;
using TwitterCloneBack.Orchestrator;

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
        catch (InvalidArgumentException ex)
        {
            logger.LogError(ex, "Invalid argument exception");
            await WriteErrorResponseAsync(context, HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (NotFoundException ex)
        {
            logger.LogError(ex, "Not found exception");
            await WriteErrorResponseAsync(context, HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (ForbiddenException ex)
        {
            logger.LogError(ex, "Forbidden exception");
            await WriteErrorResponseAsync(context, HttpStatusCode.Forbidden,
                ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteErrorResponseAsync(
                context,
                HttpStatusCode.InternalServerError,
                "Internal server error");
        }
    }

    private static async Task WriteErrorResponseAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message
    )
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response =
            new
            {
                statusCode = context.Response.StatusCode,
                message
            };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}