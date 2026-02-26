using Jalles.Core.DomainModels;

namespace Jalles.Web.Contracts;

public interface ISearchService
{
    Task<SearchResult> QueryAsync(string searchTerm, int skip, int take, string culture);
}
