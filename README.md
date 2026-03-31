# Philiprehberger.ApiErrorStandardizer

[![CI](https://github.com/philiprehberger/dotnet-api-error-standardizer/actions/workflows/ci.yml/badge.svg)](https://github.com/philiprehberger/dotnet-api-error-standardizer/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Philiprehberger.ApiErrorStandardizer.svg)](https://www.nuget.org/packages/Philiprehberger.ApiErrorStandardizer)
[![Last updated](https://img.shields.io/github/last-commit/philiprehberger/dotnet-api-error-standardizer)](https://github.com/philiprehberger/dotnet-api-error-standardizer/commits/main)

Middleware for consistent, structured JSON API error responses.

## Installation

```bash
dotnet add package Philiprehberger.ApiErrorStandardizer
```

## Usage

Register the middleware early in your pipeline in `Program.cs`:

```csharp
using Philiprehberger.ApiErrorStandardizer;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseErrorStandardizer();

app.MapGet("/example", () =>
{
    throw new KeyNotFoundException("User not found");
});

app.Run();
```

When an unhandled exception is thrown, the middleware returns a structured JSON response:

```json
{
  "error": "NotFound",
  "message": "User not found",
  "statusCode": 404,
  "traceId": "0HN4CMFQ8G5RI:00000001"
}
```

### Custom Exception Mapping

Map your own exception types to specific HTTP status codes:

```csharp
app.UseErrorStandardizer(options =>
{
    options.ExceptionStatusMap[typeof(InsufficientFundsException)] = 402;
    options.ExceptionStatusMap[typeof(RateLimitException)] = 429;
    options.IncludeStackTrace = false;
    options.IncludeTraceId = true;
});
```

### Built-in Exception Mappings

| Exception Type | Status Code |
|----------------|-------------|
| `ArgumentException` | 400 Bad Request |
| `ArgumentNullException` | 400 Bad Request |
| `ArgumentOutOfRangeException` | 400 Bad Request |
| `UnauthorizedAccessException` | 401 Unauthorized |
| `KeyNotFoundException` | 404 Not Found |
| `InvalidOperationException` | 409 Conflict |
| `NotImplementedException` | 501 Not Implemented |
| All other exceptions | 500 Internal Server Error |

Custom mappings in `ExceptionStatusMap` take precedence over built-in defaults.

## API

### `ApiError`

| Property | Type | Description |
|----------|------|-------------|
| `Error` | `string` | The error type or category |
| `Message` | `string` | Human-readable error description |
| `StatusCode` | `int` | HTTP status code |
| `TraceId` | `string?` | Request trace identifier (if enabled) |
| `ValidationErrors` | `IDictionary<string, string[]>?` | Field-level validation errors |

### `ErrorStandardizerOptions`

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `IncludeStackTrace` | `bool` | `false` | Include stack trace in responses |
| `IncludeTraceId` | `bool` | `true` | Include trace ID from request context |
| `ExceptionStatusMap` | `Dictionary<Type, int>` | empty | Custom exception-to-status-code mappings |

### `ApplicationBuilderExtensions`

| Method | Description |
|--------|-------------|
| `UseErrorStandardizer(Action<ErrorStandardizerOptions>?)` | Adds the error standardizer middleware to the pipeline |

## Development

```bash
dotnet build src/Philiprehberger.ApiErrorStandardizer.csproj --configuration Release
```

## Support

If you find this project useful:

⭐ [Star the repo](https://github.com/philiprehberger/dotnet-api-error-standardizer)

🐛 [Report issues](https://github.com/philiprehberger/dotnet-api-error-standardizer/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

💡 [Suggest features](https://github.com/philiprehberger/dotnet-api-error-standardizer/issues?q=is%3Aissue+is%3Aopen+label%3Aenhancement)

❤️ [Sponsor development](https://github.com/sponsors/philiprehberger)

🌐 [All Open Source Projects](https://philiprehberger.com/open-source-packages)

💻 [GitHub Profile](https://github.com/philiprehberger)

🔗 [LinkedIn Profile](https://www.linkedin.com/in/philiprehberger)

## License

[MIT](LICENSE)
