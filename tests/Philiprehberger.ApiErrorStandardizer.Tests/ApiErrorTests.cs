using Xunit;
using Philiprehberger.ApiErrorStandardizer;

namespace Philiprehberger.ApiErrorStandardizer.Tests;

public class ApiErrorTests
{
    [Fact]
    public void Constructor_SetsRequiredProperties()
    {
        var error = new ApiError("BadRequest", "Invalid input", 400);

        Assert.Equal("BadRequest", error.Error);
        Assert.Equal("Invalid input", error.Message);
        Assert.Equal(400, error.StatusCode);
    }

    [Fact]
    public void OptionalProperties_DefaultToNull()
    {
        var error = new ApiError("NotFound", "Not found", 404);

        Assert.Null(error.TraceId);
        Assert.Null(error.ValidationErrors);
    }

    [Fact]
    public void Constructor_WithTraceId_SetsTraceId()
    {
        var error = new ApiError("InternalServerError", "Something went wrong", 500, TraceId: "abc-123");

        Assert.Equal("abc-123", error.TraceId);
    }

    [Fact]
    public void Constructor_WithValidationErrors_SetsValidationErrors()
    {
        var validationErrors = new Dictionary<string, string[]>
        {
            ["Name"] = new[] { "Name is required" }
        };

        var error = new ApiError("BadRequest", "Validation failed", 400, ValidationErrors: validationErrors);

        Assert.NotNull(error.ValidationErrors);
        Assert.Single(error.ValidationErrors);
        Assert.Contains("Name", error.ValidationErrors.Keys);
    }
}
