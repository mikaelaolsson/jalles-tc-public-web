namespace Jalles.Core.ViewModels;

public class FooterViewModel
{
    public string? FooterText { get; set; }
    public IEnumerable<SponsorViewModel> Sponsors { get; set; } = Enumerable.Empty<SponsorViewModel>();
    public MediaWithCrops? UmemaranLogo { get; set; }
}
