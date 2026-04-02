namespace Jalles.Core.ViewModels.Blocks;

public class ContentBlockViewModel
{
    public string? Heading { get; set; }
    public string? Text { get; set; }
    public MediaBlockViewModel MediaBlock { get; set; } = new();
    public string MediaAlign { get; set; } = "Left";
    public IEnumerable<CtaBlockViewModel> CtaBlocks { get; set; } = [];
    public string BackgroundColor { get; set; } = "color-oat-milk";
}
