namespace Jalles.Core.ViewModels;

public class ListingPageViewModel : BasePageViewModel
{
    public IEnumerable<ContentPageViewModel> ContentPages { get; set; } = Enumerable.Empty<ContentPageViewModel>();
}
