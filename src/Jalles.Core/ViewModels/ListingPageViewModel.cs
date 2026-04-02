using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jalles.Core.ViewModels;

public class ListingPageViewModel : BasePageViewModel
{
    public bool DisplayTitle { get; set; } = false;
    public IEnumerable<ContentPageViewModel> ContentPages { get; set; } = [];
    public IEnumerable<SelectListItem> DisplayedCategories { get; set; } = [];
}
