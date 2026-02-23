using Jalles.TestHelpers;
using Jalles.Web.Contracts;
using Jalles.Web.Extensions;
using Jalles.Web.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Jalles.Web.Tests.Extensions;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddUmbracoServices_RegistersUmbracoServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Register all internal Umbraco fakes and logging
        services.ConfigureUmbracoFakes();
        services.AddLogging();

        // Act
        services.AddUmbracoServices();
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetRequiredService<IMixedListingBlockService>().ShouldBeOfType<MixedListingBlockService>();
    }
}
