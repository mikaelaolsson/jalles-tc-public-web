using Examine.Lucene;
using Lucene.Net.Index;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Jalles.Core.Analyzers;
using Jalles.Core.Indexing;

namespace Jalles.Core.Tests.Indexing;

public class ConfigureExternalIndexOptionsTests
{
    [Fact]
    public void Configure_ShouldSetAnalyzerToAsciiFoldingAnalyzer_WhenNameIsExternalIndex()
    {
        // Arrange
        var settings = new IndexCreatorSettings
        {
            LuceneDirectoryFactory = LuceneDirectoryFactory.SyncedTempFileSystemDirectoryFactory
        };

        var options = new LuceneDirectoryIndexOptions();
        var configureOptions = new ConfigureExternalIndexOptions(Options.Create(settings));

        // Act
        configureOptions.PostConfigure("ExternalIndex", options);

        // Assert
        options.Analyzer.ShouldBeOfType<AsciiFoldingAnalyzer>();
    }

    [Fact]
    public void Configure_ShouldNotSetAnalyzer_WhenNameIsNotExternalIndex()
    {
        // Arrange
        var fakeSettings = A.Fake<IOptions<IndexCreatorSettings>>();
        var options = new LuceneDirectoryIndexOptions();
        var configureOptions = new ConfigureExternalIndexOptions(fakeSettings);

        // Act
        configureOptions.PostConfigure("BulbasaurIndex", options);

        // Assert
        options.Analyzer.ShouldBeNull();
    }

    [Fact]
    public void Configure_ShouldSetIndexDeletionPolicy_WhenFactoryIsSyncedTempFileSystemDirectoryFactory()
    {
        // Arrange
        var settings = new IndexCreatorSettings
        {
            LuceneDirectoryFactory = LuceneDirectoryFactory.SyncedTempFileSystemDirectoryFactory
        };

        var options = new LuceneDirectoryIndexOptions();
        var configureOptions = new ConfigureExternalIndexOptions(Options.Create(settings));

        // Act
        configureOptions.PostConfigure("ExternalIndex", options);

        // Assert
        options.IndexDeletionPolicy.ShouldNotBeNull();
        options.IndexDeletionPolicy.ShouldBeOfType<SnapshotDeletionPolicy>();
    }
}
