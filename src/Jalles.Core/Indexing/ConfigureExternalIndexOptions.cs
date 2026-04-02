using Examine.Lucene;
using Jalles.Core.Analyzers;
using Lucene.Net.Index;
using Lucene.Net.Util;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;

namespace Jalles.Core.Indexing;

public class ConfigureExternalIndexOptions : IPostConfigureOptions<LuceneDirectoryIndexOptions>
{
    private readonly IOptions<IndexCreatorSettings> _settings;

    public ConfigureExternalIndexOptions(IOptions<IndexCreatorSettings> settings)
        => _settings = settings;

    public void PostConfigure(string? name, LuceneDirectoryIndexOptions options)
    {
        if(!name?.Equals("ExternalIndex", StringComparison.OrdinalIgnoreCase) ?? true)
            return;

        options.Analyzer = new AsciiFoldingAnalyzer(LuceneVersion.LUCENE_48);

        options.UnlockIndex = true;

        if(_settings.Value.LuceneDirectoryFactory == LuceneDirectoryFactory.SyncedTempFileSystemDirectoryFactory)
        {
            options.IndexDeletionPolicy = new SnapshotDeletionPolicy(new KeepOnlyLastCommitDeletionPolicy());
        }
    }
}
