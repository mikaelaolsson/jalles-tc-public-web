namespace Jalles.Core.DomainModels;

public sealed record SearchResult
{
    public SearchResult(IEnumerable<SearchResultItem> searchResultItems, int totalAvailableResults)
    {
        SearchResultItems = searchResultItems;
        TotalNumberOfItems = totalAvailableResults;
    }

    public int TotalNumberOfItems { get; init; }
    public IEnumerable<SearchResultItem> SearchResultItems { get; init; }
}
