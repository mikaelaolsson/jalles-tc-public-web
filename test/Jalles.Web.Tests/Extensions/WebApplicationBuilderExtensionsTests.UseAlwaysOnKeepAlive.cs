using Microsoft.AspNetCore.Http;
using WebApplicationBuilderExtensions = Jalles.Web.Extensions.WebApplicationBuilderExtensions;

namespace Jalles.Web.Tests.Extensions;

public partial class WebApplicationBuilderExtensionsTests
{
    [Fact]
    public void UseAlwaysOnKeepAlive_WhenAppIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => WebApplicationBuilderExtensions.UseAlwaysOnKeepAlive(null));
    }

    [Fact]
    public async Task InvokeAlwaysOnKeepAlive_WhenContextIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () =>
            await WebApplicationBuilderExtensions.InvokeAlwaysOnKeepAlive(null, async () => await Task.CompletedTask));
    }

    [Fact]
    public async Task InvokeAlwaysOnKeepAlive_WhenNextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () =>
            await WebApplicationBuilderExtensions.InvokeAlwaysOnKeepAlive(context, null));
    }

    [Theory]
    [InlineData("AlwaysOn", "/keep-alive")]
    [InlineData("SomeOtherAgent", "/test")]
    [InlineData(null, "/test")]
    public async Task InvokeAlwaysOnKeepAlive_WhenUserAgent_SetsPathAsExpected(string userAgent, string expectedPath)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = "/test";

        if(userAgent != null)
        {
            context.Request.Headers.UserAgent = userAgent;
        }

        var called = false;
        async Task Next()
        {
            called = true;
            await Task.CompletedTask;
        }

        // Act
        await WebApplicationBuilderExtensions.InvokeAlwaysOnKeepAlive(context, Next);

        // Assert
        context.Request.Path.ToString().ShouldBe(expectedPath);
        called.ShouldBeTrue();
    }
}
