using System.Net;
using System.Text.Json;

namespace FileReception.Api.Middlewares;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode status;
        string message;

        switch (exception)
        {
            case ArgumentException:
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            case InvalidOperationException:
                status = HttpStatusCode.BadRequest;
                message = exception.Message;
                break;
            default:
                status = HttpStatusCode.InternalServerError;
                message = "Erro interno do servidor";
                break;
        }

        var response = new
        {
            error = message,
            details = exception.Message
        };

        string json = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        return context.Response.WriteAsync(json);
    }
}
