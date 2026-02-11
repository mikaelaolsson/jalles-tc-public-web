namespace Jalles.Core.ViewModels.Blocks;

public class HighlightBlockViewModel
{
    public string Heading { get; set; } = string.Empty;
    public IEnumerable<ContentPageViewModel> Highlights { get; set; } = Enumerable.Empty<ContentPageViewModel>();
    public string BackgroundColor { get; set; } = "color-off-white";
}