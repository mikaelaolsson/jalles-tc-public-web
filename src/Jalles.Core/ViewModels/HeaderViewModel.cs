namespace Jalles.Core.ViewModels;

public class HeaderViewModel
{
    public MediaBlockViewModel MediaBlock { get; set; } = new();
    public IPublishedContent? Content { get; set; }
}
