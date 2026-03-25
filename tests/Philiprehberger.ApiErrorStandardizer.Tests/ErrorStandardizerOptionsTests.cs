using Xunit;
using Philiprehberger.ApiErrorStandardizer;

namespace Philiprehberger.ApiErrorStandardizer.Tests;

public class ErrorStandardizerOptionsTests
{
    [Fact]
    public void IncludeStackTrace_DefaultsToFalse()
    {
        var options = new ErrorStandardizerOptions();

        Assert.False(options.IncludeStackTrace);
    }

    [Fact]
    public void IncludeTraceId_DefaultsToTrue()
    {
        var options = new ErrorStandardizerOptions();

        Assert.True(options.IncludeTraceId);
    }

    [Fact]
    public void ExceptionStatusMap_DefaultsToEmpty()
    {
        var options = new ErrorStandardizerOptions();

        Assert.NotNull(options.ExceptionStatusMap);
        Assert.Empty(options.ExceptionStatusMap);
    }

    [Fact]
    public void ExceptionStatusMap_CanAddCustomMappings()
    {
        var options = new ErrorStandardizerOptions();
        options.ExceptionStatusMap[typeof(TimeoutException)] = 408;

        Assert.Single(options.ExceptionStatusMap);
        Assert.Equal(408, options.ExceptionStatusMap[typeof(TimeoutException)]);
    }
}
