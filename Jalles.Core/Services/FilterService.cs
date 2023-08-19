using Jalles.Core.Contracts;
using Jalles.Core.ViewModels;

namespace Jalles.Core.Services;

public class FilterService : IFilterService
{
    public IEnumerable<ContentPageViewModel> GetFilteredContentPages(IEnumerable<ContentPageViewModel> contentPages, string category)
    {
        if (string.IsNullOrEmpty(category)) return contentPages;

        var filteredContentPages = contentPages
            .Where(p => p.Categories.Any(c => c == category));

        return filteredContentPages;
    }
}