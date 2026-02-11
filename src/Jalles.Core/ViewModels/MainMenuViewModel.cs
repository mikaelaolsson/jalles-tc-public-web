using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Core.ViewModels;

public class MainMenuViewModel
{
    public string StartPageUrl { get; set; } = string.Empty;
    public string StartPageTitle { get; set; } = string.Empty;
    public IReadOnlyCollection<BasePageViewModel> MenuItems { get; set; } = [];
    public string Facebook { get; set; } = string.Empty;
}
