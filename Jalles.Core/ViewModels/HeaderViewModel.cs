using Jalles.Core.ViewModels.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Core.ViewModels;

public class HeaderViewModel
{
    public MediaViewModel Media { get; set; } = new();
    public IPublishedContent? Content { get; set; }
}
