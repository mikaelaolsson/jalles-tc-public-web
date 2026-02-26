using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.Miscellaneous;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis;
using Lucene.Net.Util;

namespace Jalles.Core.Analyzers;

public class AsciiFoldingAnalyzer : Analyzer
{
    private readonly LuceneVersion _version;

    public AsciiFoldingAnalyzer(LuceneVersion version)
    {
        _version = version;
    }

    protected override TokenStreamComponents CreateComponents(string fieldName, TextReader reader)
    {
        var tokenizer = new StandardTokenizer(_version, reader);
        TokenStream filter = new LowerCaseFilter(_version, tokenizer);
        filter = new ASCIIFoldingFilter(filter);
        return new TokenStreamComponents(tokenizer, filter);
    }
}
