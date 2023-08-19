using Jalles.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jalles.Core.ViewModels;

public class ListingPageViewModel : BasePageViewModel
{
    public bool DisplayTitle { get; set; } = false;
    public IEnumerable<ContentPageViewModel> ContentPages { get; set; } = Enumerable.Empty<ContentPageViewModel>();
    public IEnumerable<SelectListItem> DisplayedCategories { get; set; } = Enumerable.Empty<SelectListItem>();
    public IEnumerable<string> AllCategories { get; set; } = Enumerable.Empty<string>();
    public string SelectedCategory { get; set; } = "Alla";
    public int Page { get; set; } = 1;
    public Pagination? Pagination { get; set; }
}
