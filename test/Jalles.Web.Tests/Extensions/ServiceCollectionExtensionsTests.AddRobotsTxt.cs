using RobotsTxt;
using ServiceCollectionExtensions = Jalles.Web.Extensions.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;
using Jalles.Core.Constants;

namespace Jalles.Web.Tests.Extensions;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddRobotsTxt_WhenServicesIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection services = null;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => ServiceCollectionExtensions.AddRobotsTxt(services));
    }

    [Fact]
    public void BuildJallesRobotsTxt_CreatesExpectedRobotsTxt()
    {
        // Arrange
        var builder = new RobotsTxtOptionsBuilder();

        // Act
        var result = ServiceCollectionExtensions.BuildJallesRobotsTxt(builder);

        // Assert
        var robots = result.Build();
        robots.Environment.ShouldBe("Production");
        robots.Hostnames.ShouldContain(JallesConstants.PublicDomain);
        robots.Hostnames.ShouldContain(JallesConstants.PublicDomainWithoutWww);
        robots.SitemapUrls.ShouldContain($"https://{JallesConstants.PublicDomain}/sitemap");
        robots.Sections.Count.ShouldBe(1);

        var section = robots.Sections[0];
        section.UserAgents.ShouldContain("*");

        var disallows = section.Rules.OfType<RobotsTxtDisallowRule>().Select(r => r.Value).ToList();
        disallows.ShouldContain("/app_data/");
        disallows.ShouldContain("/app_plugins/");
        disallows.ShouldContain("/umbraco/");
        disallows.ShouldContain("/usync/");
        disallows.ShouldContain("/install");
    }
}
