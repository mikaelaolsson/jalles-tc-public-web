using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.ViewModels;

public class StartPageViewModel
{
    public MediaWithCrops? Thumbnail { get; set; }
    public IEnumerable<BlockListItem> Blocks { get; set; } = Enumerable.Empty<BlockListItem>();
}
