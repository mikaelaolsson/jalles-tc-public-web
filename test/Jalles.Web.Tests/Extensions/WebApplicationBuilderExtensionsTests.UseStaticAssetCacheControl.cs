using Microsoft.AspNetCore.Http;
using WebApplicationBuilderExtensions = Jalles.Web.Extensions.WebApplicationBuilderExtensions;

namespace Jalles.Web.Tests.Extensions;

public partial class WebApplicationBuilderExtensionsTests
{
    [Fact]
    public void UseStaticAssetCacheControl_WhenAppIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => WebApplicationBuilderExtensions.UseStaticAssetCacheControl(null));
    }

    [Fact]
    public async Task InvokeStaticAssetCacheControl_WhenContextIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () =>
            await WebApplicationBuilderExtensions.InvokeStaticAssetCacheControl(null, async () => await Task.CompletedTask));
    }

    [Fact]
    public async Task InvokeStaticAssetCacheControl_WhenNextIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        var context = new DefaultHttpContext();

        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () =>
            await WebApplicationBuilderExtensions.InvokeStaticAssetCacheControl(context, null));
    }

    [Theory]
    [InlineData("/main.js")]
    [InlineData("/main.css")]
    [InlineData("/font.woff")]
    [InlineData("/font.woff2")]
    [InlineData("/vector.svgz")]
    [InlineData("/vector.svg")]
    [InlineData("/media/1234/image.png")]
    public async Task InvokeStaticAssetCacheControl_WhenCachablePath_SetsCacheHeader(string path)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        var called = false;
        async Task Next() { called = true; await Task.CompletedTask; }

        // Act
        await WebApplicationBuilderExtensions.InvokeStaticAssetCacheControl(context, Next);

        // Assert
        context.Response.Headers.ContainsKey("Cache-Control").ShouldBeTrue();
        context.Response.Headers.CacheControl.ToString().ShouldBe("public, max-age=31536000");
        called.ShouldBeTrue();
    }

    [Theory]
    [InlineData("/other/file.txt")]
    [InlineData("/umbraco/backoffice.js")]
    public async Task InvokeStaticAssetCacheControl_WhenNonCachablePath_DoesNotSetCacheHeader(string path)
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Request.Path = path;
        var called = false;
        async Task Next() { called = true; await Task.CompletedTask; }

        // Act
        await WebApplicationBuilderExtensions.InvokeStaticAssetCacheControl(context, Next);

        // Assert
        context.Response.Headers.ContainsKey("Cache-Control").ShouldBeFalse();
        called.ShouldBeTrue();
    }
}
