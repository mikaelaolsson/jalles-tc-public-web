namespace Jalles.Core.ViewModels.Blocks;

public class ContentBlockViewModel
{
    public string? Heading { get; set; }
    public string? Text { get; set; }
    public MediaViewModel Media { get; set; } = new();
    public string MediaAlign { get; set; } = "Left";
    public IEnumerable<CtaBlockViewModel> CtaBlocks { get; set; } = Enumerable.Empty<CtaBlockViewModel>();
    public string BackgroundColor { get; set; } = "#FAFAEC";
}