using Microsoft.AspNetCore.HttpOverrides;
using ApplicationBuilderExtensions = Jalles.Web.Extensions.ApplicationBuilderExtensions;

namespace Jalles.Web.Tests.Extensions;

public partial class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void UseTrustedForwardedHeaders_WhenAppIsNull_ShouldThrow()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => ApplicationBuilderExtensions.UseTrustedForwardedHeaders(null));
    }

    [Fact]
    public void BuildTrustedForwardedHeadersOptions_ShouldReturnCorrectOptions()
    {
        // Act
        var options = ApplicationBuilderExtensions.BuildTrustedForwardedHeadersOptions();

        // Assert
        options.ShouldNotBeNull();
        options.ForwardedHeaders.ShouldBe(ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto);
        options.KnownIPNetworks.ShouldBeEmpty();
        options.KnownProxies.ShouldBeEmpty();
    }
}
