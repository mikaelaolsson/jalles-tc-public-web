namespace Jalles.Core.Contracts;

public interface IFilterService
{
    IEnumerable<ContentPageViewModel> GetFilteredContentPages(IEnumerable<ContentPageViewModel> contentPages, string? category);
}
