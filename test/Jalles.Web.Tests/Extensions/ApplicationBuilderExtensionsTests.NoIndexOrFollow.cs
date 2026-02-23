using Jalles.Core.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using ApplicationBuilderExtensions = Jalles.Web.Extensions.ApplicationBuilderExtensions;

namespace Jalles.Web.Tests.Extensions;

public partial class ApplicationBuilderExtensionsTests
{

    [Fact]
    public void NoIndexOrFollow_WhenCalled_RegistersMiddlewareWithAppBuilder()
    {
        // Arrange
        var appBuilder = A.Fake<IApplicationBuilder>();
        Func<RequestDelegate, RequestDelegate> captured = null;
        A.CallTo(() => appBuilder.Use(A<Func<RequestDelegate, RequestDelegate>>.Ignored))
            .WhenArgumentsMatch(args =>
            {
                captured = args.Get<Func<RequestDelegate, RequestDelegate>>(0);
                return true;
            })
            .ReturnsLazily(_ => appBuilder);

        // Act
        var result = ApplicationBuilderExtensions.NoIndexOrFollow(appBuilder);

        // Assert
        result.ShouldBe(appBuilder);
        captured.ShouldNotBeNull();
    }

    [Fact]
    public void NoIndexOrFollow_WhenAppIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => ApplicationBuilderExtensions.NoIndexOrFollow(null));
    }

    [Theory]
    [InlineData("notpublic.com")]
    [InlineData("test.jallestc.se")]
    public async Task NoIndexOrFollow_WhenNonPublicDomain_SetsHeader(string host)
    {
        // Arrange
        var appBuilder = A.Fake<IApplicationBuilder>();
        Func<RequestDelegate, RequestDelegate> captured = null;
        A.CallTo(() => appBuilder.Use(A<Func<RequestDelegate, RequestDelegate>>.Ignored))
            .WhenArgumentsMatch(args =>
            {
                captured = args.Get<Func<RequestDelegate, RequestDelegate>>(0);
                return true;
            })
            .ReturnsLazily(_ => appBuilder);
        ApplicationBuilderExtensions.NoIndexOrFollow(appBuilder);
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString(host);
        var next = new RequestDelegate(_ => Task.CompletedTask);

        // Act
        await captured(next)(context);

        // Assert
        context.Response.Headers.ContainsKey("X-Robots-Tag").ShouldBeTrue();
        context.Response.Headers["X-Robots-Tag"].ToString().ShouldBe("noindex, nofollow");
    }

    [Theory]
    [InlineData(JallesConstants.PublicDomain)]
    [InlineData(JallesConstants.PublicDomainWithoutWww)]
    public async Task NoIndexOrFollow_WhenPublicDomain_DoesNotSetHeader(string host)
    {
        // Arrange
        var appBuilder = A.Fake<IApplicationBuilder>();
        Func<RequestDelegate, RequestDelegate> captured = null;
        A.CallTo(() => appBuilder.Use(A<Func<RequestDelegate, RequestDelegate>>.Ignored))
            .WhenArgumentsMatch(args =>
            {
                captured = args.Get<Func<RequestDelegate, RequestDelegate>>(0);
                return true;
            })
            .ReturnsLazily(_ => appBuilder);

        ApplicationBuilderExtensions.NoIndexOrFollow(appBuilder);
        var context = new DefaultHttpContext();
        context.Request.Host = new HostString(host);
        var next = new RequestDelegate(_ => Task.CompletedTask);

        // Act
        await captured(next)(context);

        // Assert
        context.Response.Headers.ContainsKey("X-Robots-Tag").ShouldBeFalse();
    }
}
