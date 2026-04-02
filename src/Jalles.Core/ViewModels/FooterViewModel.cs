namespace Jalles.Core.ViewModels;

public class FooterViewModel
{
    public string? FooterText { get; set; }
    public IEnumerable<SponsorViewModel> Sponsors { get; set; } = [];
    public string UmemaranLogoSource { get; set; } = string.Empty;
}
