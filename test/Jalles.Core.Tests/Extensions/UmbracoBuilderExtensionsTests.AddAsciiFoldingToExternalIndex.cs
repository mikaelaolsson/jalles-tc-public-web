using Examine.Lucene;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Jalles.Core.Extensions;
using Jalles.Core.Indexing;

namespace Jalles.Core.Tests.Extensions;

public partial class UmbracoBuilderExtensionsTests
{
    [Fact]
    public void AddAsciiFoldingToExternalIndex_ShouldRegisterConfigureExternalIndexOptions()
    {
        // Arrange
        var services = new ServiceCollection();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection([])
            .Build();

        var typeLoader = A.Dummy<TypeLoader>();
        var builder = new UmbracoBuilder(services, config, typeLoader);

        // Act
        builder.AddAsciiFoldingToExternalIndex();

        // Assert
        var provider = services.BuildServiceProvider();

        var configureOptions = provider.GetServices<IPostConfigureOptions<LuceneDirectoryIndexOptions>>();
        configureOptions.ShouldContain(x => x is ConfigureExternalIndexOptions);
    }
}
