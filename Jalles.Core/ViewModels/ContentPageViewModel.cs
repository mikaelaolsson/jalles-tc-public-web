using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.ViewModels;

public class ContentPageViewModel : BasePageViewModel
{
    public DateTime LastEdited { get; set; }
    public DateTime Published { get; set; }
    public IEnumerable<BlockListItem> Blocks { get; set; } = Enumerable.Empty<BlockListItem>();
}
