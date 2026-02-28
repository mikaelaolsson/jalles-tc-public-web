namespace Jalles.Core.ViewModels;

public class HeaderViewModel
{
    public MediaBlockViewModel MediaBlock { get; set; } = new();
    public string Heading { get; set; } = string.Empty;
    public string SubHeading { get; set; } = string.Empty;
}
