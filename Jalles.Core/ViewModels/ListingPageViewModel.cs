namespace Jalles.Core.ViewModels;

public class ListingPageViewModel : BasePageViewModel
{
    public bool DisplayTitle { get; set; } = false;
    public IEnumerable<ContentPageViewModel> ContentPages { get; set; } = Enumerable.Empty<ContentPageViewModel>();
}
