namespace Philiprehberger.ApiErrorStandardizer;

/// <summary>
/// Represents a standardized API error response.
/// </summary>
/// <param name="Error">The error type or category.</param>
/// <param name="Message">A human-readable description of the error.</param>
/// <param name="StatusCode">The HTTP status code associated with the error.</param>
/// <param name="TraceId">An optional trace identifier for correlating logs and requests.</param>
/// <param name="ValidationErrors">An optional dictionary of field-level validation errors.</param>
public record ApiError(
    string Error,
    string Message,
    int StatusCode,
    string? TraceId = null,
    IDictionary<string, string[]>? ValidationErrors = null
);
