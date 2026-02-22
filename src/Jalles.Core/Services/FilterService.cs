using Jalles.Core.Contracts;

namespace Jalles.Core.Services;

public class FilterService : IFilterService
{
    public IEnumerable<ContentPageViewModel> GetFilteredContentPages(IEnumerable<ContentPageViewModel> contentPages, string? category)
    {
        if(string.IsNullOrEmpty(category) || category == "Alla") return contentPages;

        return contentPages
            .Where(p => p.Categories.Any(c => c == category));
    }
}
