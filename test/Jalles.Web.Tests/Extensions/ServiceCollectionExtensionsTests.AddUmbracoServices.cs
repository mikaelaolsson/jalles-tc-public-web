using Jalles.Core.Extensions;
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
        services.AddCoreServices();
        services.AddUmbracoServices();
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetRequiredService<IMixedListingBlockService>().ShouldBeOfType<MixedListingBlockService>();
        provider.GetRequiredService<ExcelBlockDataLoader>().ShouldBeOfType<ExcelBlockDataLoader>();
        provider.GetRequiredService<ISearchService>().ShouldBeOfType<SearchService>();
    }
}
