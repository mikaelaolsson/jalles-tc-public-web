using Jalles.Web.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shorthand.Vite;

namespace Jalles.Web.Tests.Extensions;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddViteWithDefaults_RegistersViteWithExpectedOptions()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddViteWithDefaults();
        var provider = services.BuildServiceProvider();

        // Assert
        var options = provider.GetService<IOptions<ViteOptions>>();
        options.ShouldNotBeNull();

        var viteOptions = options.Value;
        viteOptions.ShouldNotBeNull();
        viteOptions.ManifestFileName.ShouldBe(".vite/manifest.json");
        viteOptions.Port.ShouldBe(5010);
        viteOptions.Https.ShouldBe(true);
    }
}
