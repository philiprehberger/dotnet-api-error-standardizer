using Microsoft.AspNetCore.Builder;

namespace Philiprehberger.ApiErrorStandardizer;

/// <summary>
/// Extension methods for adding the error standardizer middleware to the application pipeline.
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds the error standardizer middleware to the application's request pipeline.
    /// This middleware catches unhandled exceptions and returns consistent JSON error responses.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="configure">An optional action to configure <see cref="ErrorStandardizerOptions"/>.</param>
    /// <returns>The application builder for chaining.</returns>
    public static IApplicationBuilder UseErrorStandardizer(
        this IApplicationBuilder app,
        Action<ErrorStandardizerOptions>? configure = null)
    {
        var options = new ErrorStandardizerOptions();
        configure?.Invoke(options);

        return app.UseMiddleware<ErrorStandardizerMiddleware>(options);
    }
}
