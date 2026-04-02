namespace Jalles.Core.ViewModels;

public class SecondaryListingPageViewModel : ListingPageViewModel
{
    public PinThisBlockViewModel? PinThisBlock { get; set; }
    public string MainCategory { get; set; } = "";
}
