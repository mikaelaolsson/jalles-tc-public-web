using Jalles.Core.Contracts;
using Jalles.Core.Utilities;

namespace Jalles.Core.Services;

public class PaginationService : IPaginationService
{
    public ListingPageViewModel GetPaginatedViewModel(ListingPageViewModel viewModel, int page)
    {
        viewModel.Page = page;

        var contentPageViewModels = viewModel.ContentPages.ToList();

        viewModel.Pagination = new Pagination(contentPageViewModels.Count, viewModel.Page);
        viewModel.ContentPages = Paginate(contentPageViewModels, viewModel.Pagination);

        return viewModel;
    }

    public SecondaryListingPageViewModel GetPaginatedViewModel(SecondaryListingPageViewModel viewModel, int page)
    {
        viewModel.Page = page;

        var contentPageViewModels = viewModel.ContentPages.ToList();

        viewModel.Pagination = new Pagination(contentPageViewModels.Count, viewModel.Page);
        viewModel.ContentPages = Paginate(contentPageViewModels, viewModel.Pagination);

        return viewModel;
    }

    private static IEnumerable<ContentPageViewModel> Paginate(IEnumerable<ContentPageViewModel> articles, Pagination pagination)
    {
        var skips = pagination.PageSize * (pagination.Page - 1);

        return articles.Skip(skips).Take(pagination.PageSize);
    }
}
