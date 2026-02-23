using Jalles.Web.Rules;
using ApplicationBuilderExtensions = Jalles.Web.Extensions.ApplicationBuilderExtensions;

namespace Jalles.Web.Tests.Extensions;

public partial class ApplicationBuilderExtensionsTests
{
    [Fact]
    public void UseRewriteRules_WhenAppIsNull_ThrowsArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => ApplicationBuilderExtensions.UseRewriteRules(null));
    }

    [Fact]
    public void BuildRewriteOptions_AddsWwwAndAzureRules()
    {
        // Act
        var options = ApplicationBuilderExtensions.BuildRewriteOptions();

        // Assert
        options.ShouldNotBeNull();
        options.Rules.Count.ShouldBe(2);
        options.Rules[0].GetType().Name.ShouldBe("RedirectToWwwRule");
        options.Rules[1].ShouldBeOfType<RedirectFromAzureWebsites>();
        options.Rules.ShouldContain(r => r is RedirectFromAzureWebsites);
    }
}
