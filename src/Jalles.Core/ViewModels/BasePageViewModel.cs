using Jalles.Core.ViewModels.Blocks;

namespace Jalles.Core.ViewModels;

public class BasePageViewModel
{
    public BasePageViewModel()
    {
        Header = new MediaBlockViewModel();
    }

    public Guid Guid { get; set; }
    public string PagePath { get; set; } = "";
    public string ParentPagePath { get; set; } = "";
    public string Title { get; set; } = "";
    public MediaBlockViewModel Header { get; set; }
    public MediaWithCrops? Thumbnail { get; set; }
    public string MetaDescription { get; set; } = "";
}
