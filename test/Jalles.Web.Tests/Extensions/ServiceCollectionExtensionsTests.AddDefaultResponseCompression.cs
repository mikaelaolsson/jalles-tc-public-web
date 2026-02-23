using Microsoft.AspNetCore.ResponseCompression;
using ServiceCollectionExtensions = Jalles.Web.Extensions.ServiceCollectionExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace Jalles.Web.Tests.Extensions;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDefaultResponseCompression_WhenServicesIsNull_ThrowsArgumentNullException()
    {
        // Arrange
        IServiceCollection services = null;

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => ServiceCollectionExtensions.AddDefaultResponseCompression(services));
    }

    [Fact]
    public void ConfigureDefaultResponseCompressionOptions_ConfiguresOptionsCorrectly()
    {
        // Arrange
        var options = new ResponseCompressionOptions();

        // Act
        ServiceCollectionExtensions.ConfigureDefaultResponseCompressionOptions(options);

        // Assert
        options.EnableForHttps.ShouldBeTrue();

        var providerFactories = options.Providers
            .Where(p => p.GetType().Name == "CompressionProviderFactory")
            .ToList();

        providerFactories.ShouldNotBeEmpty();

        var providerTypeProp = providerFactories[0].GetType().GetProperty("ProviderType", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        providerTypeProp.ShouldNotBeNull();

        var hasBrotli = providerFactories.Any(f => (Type)providerTypeProp.GetValue(f) == typeof(BrotliCompressionProvider));
        var hasGzip = providerFactories.Any(f => (Type)providerTypeProp.GetValue(f) == typeof(GzipCompressionProvider));

        hasBrotli.ShouldBeTrue();
        hasGzip.ShouldBeTrue();
        options.MimeTypes.ShouldContain(m => m == "image/svg+xml");
    }
}
