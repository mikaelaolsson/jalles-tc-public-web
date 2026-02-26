using Jalles.Core.Contracts;
using Jalles.Core.Extensions;
using Jalles.Core.Services;
using Jalles.TestHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Jalles.Core.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCoreServices_RegistersCoreServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Register all internal Umbraco fakes and logging
        services.AddLogging();
        services.ConfigureUmbracoFakes();

        // Act
        services.AddCoreServices();
        var provider = services.BuildServiceProvider();

        // Assert
        provider.GetRequiredService<IContentAccessor>().ShouldBeOfType<ContentAccessor>();
        provider.GetRequiredService<IFilterService>().ShouldBeOfType<FilterService>();
        provider.GetRequiredService<ILayoutViewModelService>().ShouldBeOfType<LayoutViewModelService>();
        provider.GetRequiredService<ISitemapService>().ShouldBeOfType<SitemapService>();
        provider.GetRequiredService<IUmbracoPagePathService>().ShouldBeOfType<UmbracoPagePathService>();
        provider.GetRequiredService<IExcelService>().ShouldBeOfType<ExcelService>();
        provider.GetRequiredService<IHttpClientFactory>().ShouldNotBeNull();
    }
}
