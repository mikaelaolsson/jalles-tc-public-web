using Jalles.Core.ViewModels.MediaTypes;

namespace Jalles.Core.ViewModels.Blocks;

public class SponsorBlockViewModel
{
    public string Heading { get; set; } = string.Empty;
    public IEnumerable<SponsorViewModel> Sponsors { get; set; } = Enumerable.Empty<SponsorViewModel>();
}