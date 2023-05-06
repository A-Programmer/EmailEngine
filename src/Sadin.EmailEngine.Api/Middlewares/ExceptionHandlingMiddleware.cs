using System.Collections.Concurrent;
using System.Net;
using Humanizer;

namespace Sadin.EmailEngine.Api.Middlewares;

public static class ExceptionMiddlewareExtension
{
    public static void UseCustomExceptionHandling(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
internal sealed class ExceptionHandlingMiddleware : IMiddleware
{
    private readonly IExceptionToResponseMapper _exceptionToResponseMapper;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger,
        IExceptionToResponseMapper exceptionToResponseMapper)
    {
        _logger = logger;
        _exceptionToResponseMapper = exceptionToResponseMapper;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
            await HandleExceptionAsync(context, e);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        ExceptionResponse errorResponse = _exceptionToResponseMapper.Map(exception);
        context.Response.StatusCode = (int) (errorResponse?.StatusCode
                                             ?? HttpStatusCode.InternalServerError);
        var response = errorResponse?.Response;
        if (response is null)
        {
            return;
        }
            
        await context.Response.WriteAsJsonAsync(response);
    }
}

public sealed record ExceptionResponse(object Response, HttpStatusCode StatusCode);
internal interface IExceptionToResponseMapper
{
    ExceptionResponse Map(Exception exception);
}
internal sealed class ExceptionToResponseMapper : IExceptionToResponseMapper
{
    private static readonly ConcurrentDictionary<Type, string> Codes = new();

    public ExceptionResponse Map(Exception exception)
        => exception switch
        {
            Exception ex => new ExceptionResponse(new ErrorsResponse(new Error(GetErrorCode(ex), ex.Message))
                , HttpStatusCode.BadRequest),
            _ => new ExceptionResponse(new ErrorsResponse(new Error("error", "There was an error.")),
                HttpStatusCode.InternalServerError)
        };

    private record Error(string Code, string Message);

    private record ErrorsResponse(params Error[] Errors);

    private static string GetErrorCode(object exception)
    {
        var type = exception.GetType();
        return Codes.GetOrAdd(type, type.Name.Underscore().Replace("_exception", string.Empty));
    }
}