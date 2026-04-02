using Microsoft.AspNetCore.Http;
using WebApplicationBuilderExtensions = Jalles.Web.Extensions.WebApplicationBuilderExtensions;

namespace Jalles.Web.Tests.Extensions;

public partial class WebApplicationBuilderExtensionsTests
{
    [Fact]
    public void SetSecurityHeaders_WhenFrontendRequest_SetsExpectedHeaders()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/";

        // Act
        WebApplicationBuilderExtensions.SetSecurityHeaders(context);

        // Assert
        context.Response.Headers.XContentTypeOptions.ToString().ShouldBe("nosniff");
        context.Response.Headers.XFrameOptions.ToString().ShouldBe("SAMEORIGIN");
        context.Response.Headers["Referrer-Policy"].ToString().ShouldBe("strict-origin-when-cross-origin");
        context.Response.Headers["Permissions-Policy"].ToString().ShouldBe("geolocation=(), camera=(), microphone=(), payment=(), usb=(), fullscreen=()");
        context.Response.Headers.ContentSecurityPolicy.ToString().ShouldContain("default-src data: blob: filesystem: about: ws: wss: frame-src: * 'unsafe-inline' 'unsafe-eval';");
    }

    [Theory]
    [InlineData("/umbraco")]
    [InlineData("/umbraco/backoffice")]
    [InlineData("/App_Plugins")]
    [InlineData("/App_Plugins/foo")]
    public void SetSecurityHeaders_WhenExceptedPath_DoesNotSetCsp(string path)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = path;

        // Act
        WebApplicationBuilderExtensions.SetSecurityHeaders(context);

        // Assert
        context.Response.Headers.ContentSecurityPolicy.ToString().ShouldBe("");
    }

    [Fact]
    public void UseSecurityHeaders_WhenAppIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => WebApplicationBuilderExtensions.UseSecurityHeaders(null));
    }
}
