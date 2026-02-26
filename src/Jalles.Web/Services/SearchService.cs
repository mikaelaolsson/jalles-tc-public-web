using System.ComponentModel;
using System.Reflection;
using AutoMapper;
using Examine;
using Examine.Search;
using Jalles.Core.Constants;
using Jalles.Core.DomainModels;
using Jalles.Core.Enum;
using Jalles.Core.Helpers;
using Jalles.Web.Contracts;
using Umbraco.Cms.Core;
using Umbraco.Cms.Infrastructure.Examine;
using static Umbraco.Cms.Core.Constants;
using SearchResult = Jalles.Core.DomainModels.SearchResult;

namespace Jalles.Web.Services;

public class SearchService : ISearchService
{
    private readonly IExamineManager _examineManager;
    private readonly IMapper _mapper;
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly ILogger<SearchService> _logger;

    public SearchService(
        IExamineManager examineManager,
        IMapper mapper,
        IPublishedContentQuery publishedContentQuery,
        ILogger<SearchService> logger)
    {
        _examineManager = examineManager;
        _mapper = mapper;
        _publishedContentQuery = publishedContentQuery;
        _logger = logger;
    }

    public async Task<SearchResult> QueryAsync(string searchTerm, int skip, int take, string culture)
    {
        var results = new SearchResult([], 0);

        if(string.IsNullOrWhiteSpace(searchTerm))
            return results;

        try
        {
            if(!_examineManager.TryGetIndex(UmbracoIndexes.ExternalIndexName, out var index))
            {
                throw new InvalidOperationException("No index found by name ExternalIndex.");
            }

            var searcher = index.Searcher;

            var query = searcher.CreateQuery(IndexTypes.Content)
                .GroupedOr([UmbracoExamineFieldNames.ItemTypeFieldName], SearchFieldConstants.SearchablePageAliases);

            var foldedTerms = searchTerm
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(AsciiHelper.FoldToAscii)
                .ToArray();

            query = foldedTerms.Aggregate(query, (current, term) =>
                current.And().GroupedOr(SearchFieldConstants.Fields, term.MultipleCharacterWildcard()));

            var resultsAsSearchItems = query.Execute(QueryOptions.SkipTake(skip, take)).ToArray();
            var totalNumberOfItems = query.Execute().ToArray().Length;

            var ids = resultsAsSearchItems.Select(x => int.Parse(x.Id)).ToArray();
            var content = ids.Select(_publishedContentQuery.Content).ToArray();

            results = MapToSearchResult(content, totalNumberOfItems);
        }
        catch(Exception ex)
        {
            _logger.LogWarning(ex, "Search results threw unexpected exception.");
        }

        return results;
    }

    private SearchResult MapToSearchResult(IEnumerable<IPublishedContent?> results, int totalNumberOfItems)
    {
        var searchItems = new List<SearchResultItem>();

        foreach(var result in results)
        {
            if(result == null)
                continue;

            var aliasFirstUpper = result.ContentType.Alias is { Length: > 0 } alias
                ? char.ToUpperInvariant(alias[0]) + alias[1..]
                : null;

            if(aliasFirstUpper == null || !Enum.TryParse(aliasFirstUpper, out SearchablePages pageType))
                continue;

            var searchItem = _mapper.Map<SearchResultItem>(result);

            if(searchItem == null)
                continue;

            searchItem.ContentTypeTagName = GetDescription(pageType);
            searchItems.Add(searchItem);
        }

        return new SearchResult(searchItems, totalNumberOfItems);
    }

    private static string GetDescription(SearchablePages pageType)
    {
        return pageType.GetType()
            .GetField(pageType.ToString())
            ?.GetCustomAttribute<DescriptionAttribute>()
            ?.Description ?? string.Empty;
    }
}
