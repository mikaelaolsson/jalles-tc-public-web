using Jalles.Core.ViewModels;

namespace Jalles.Core.Contracts;

public interface IPaginationService
{
    ListingPageViewModel GetPaginatedViewModel(ListingPageViewModel viewModel, int page);
    SecondaryListingPageViewModel GetPaginatedViewModel(SecondaryListingPageViewModel viewModel, int page);
}