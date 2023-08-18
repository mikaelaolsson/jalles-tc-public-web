using Jalles.Core.ViewModels;

namespace Jalles.Core.Contracts;

public interface IPaginationService
{
    ListingPageViewModel GetPaginatedViewModel(ListingPageViewModel viewModel, ListingPageViewModel request);
}