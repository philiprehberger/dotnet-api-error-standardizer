namespace Philiprehberger.ApiErrorStandardizer;

/// <summary>
/// Configuration options for the error standardizer middleware.
/// </summary>
public class ErrorStandardizerOptions
{
    /// <summary>
    /// Gets or sets whether to include the stack trace in error responses. Default is <c>false</c>.
    /// </summary>
    public bool IncludeStackTrace { get; set; } = false;

    /// <summary>
    /// Gets or sets whether to include the trace identifier in error responses. Default is <c>true</c>.
    /// </summary>
    public bool IncludeTraceId { get; set; } = true;

    /// <summary>
    /// Gets a dictionary mapping exception types to HTTP status codes.
    /// Custom mappings take precedence over built-in defaults.
    /// </summary>
    public Dictionary<Type, int> ExceptionStatusMap { get; } = new();
}
