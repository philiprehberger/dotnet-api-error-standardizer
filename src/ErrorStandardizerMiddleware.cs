using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Philiprehberger.ApiErrorStandardizer;

/// <summary>
/// Middleware that catches unhandled exceptions and writes a standardized JSON error response.
/// </summary>
public class ErrorStandardizerMiddleware
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
    };

    private static readonly Dictionary<Type, int> DefaultStatusMap = new()
    {
        [typeof(ArgumentException)] = StatusCodes.Status400BadRequest,
        [typeof(ArgumentNullException)] = StatusCodes.Status400BadRequest,
        [typeof(ArgumentOutOfRangeException)] = StatusCodes.Status400BadRequest,
        [typeof(UnauthorizedAccessException)] = StatusCodes.Status401Unauthorized,
        [typeof(KeyNotFoundException)] = StatusCodes.Status404NotFound,
        [typeof(InvalidOperationException)] = StatusCodes.Status409Conflict,
        [typeof(NotImplementedException)] = StatusCodes.Status501NotImplemented
    };

    private readonly RequestDelegate _next;
    private readonly ErrorStandardizerOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorStandardizerMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="options">The error standardizer options.</param>
    public ErrorStandardizerMiddleware(RequestDelegate next, ErrorStandardizerOptions options)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    /// <summary>
    /// Invokes the middleware. Catches unhandled exceptions and writes a standardized JSON error response.
    /// </summary>
    /// <param name="context">The HTTP context for the current request.</param>
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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = ResolveStatusCode(exception);
        var errorName = ResolveErrorName(statusCode);

        var message = exception.Message;
        if (_options.IncludeStackTrace && exception.StackTrace is not null)
        {
            message = $"{exception.Message}\n{exception.StackTrace}";
        }

        var traceId = _options.IncludeTraceId ? context.TraceIdentifier : null;

        var apiError = new ApiError(
            Error: errorName,
            Message: message,
            StatusCode: statusCode,
            TraceId: traceId
        );

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(apiError, JsonOptions);
        await context.Response.WriteAsync(json);
    }

    private int ResolveStatusCode(Exception exception)
    {
        var exceptionType = exception.GetType();

        if (_options.ExceptionStatusMap.TryGetValue(exceptionType, out var customStatus))
        {
            return customStatus;
        }

        if (DefaultStatusMap.TryGetValue(exceptionType, out var defaultStatus))
        {
            return defaultStatus;
        }

        return StatusCodes.Status500InternalServerError;
    }

    private static string ResolveErrorName(int statusCode) => statusCode switch
    {
        400 => "BadRequest",
        401 => "Unauthorized",
        403 => "Forbidden",
        404 => "NotFound",
        409 => "Conflict",
        422 => "UnprocessableEntity",
        429 => "TooManyRequests",
        501 => "NotImplemented",
        503 => "ServiceUnavailable",
        _ => "InternalServerError"
    };
}
