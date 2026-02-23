using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebApplicationBuilderExtensions = Jalles.Web.Extensions.WebApplicationBuilderExtensions;

namespace Jalles.Web.Tests.Extensions;

public partial class WebApplicationBuilderExtensionsTests
{
    [Fact]
    public void ConfigureKestrelLimitsOptions_WhenConfigIsNull_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Should.Throw<ArgumentNullException>(() => WebApplicationBuilderExtensions.ConfigureKestrelLimitsOptions(null));
    }

    [Fact]
    public void ConfigureKestrelLimitsOptions_WhenCalled_SetsExpectedLimits()
    {
        // Arrange
        var options = new KestrelServerOptions();

        // Act
        WebApplicationBuilderExtensions.ConfigureKestrelLimitsOptions(options);

        // Assert
        options.Limits.MaxRequestBodySize.ShouldBe(52_428_800 * 10);
        options.AddServerHeader.ShouldBeFalse();
    }

    [Fact]
    public void ConfigureKestrelLimits_WhenBuilderIsNull_ThrowsArgumentNullException()
    {
        // Arrange, Act & Assert
        Should.Throw<ArgumentNullException>(() => WebApplicationBuilderExtensions.ConfigureKestrelLimits(null));
    }
}
