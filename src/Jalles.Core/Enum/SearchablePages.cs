using System.ComponentModel;

namespace Jalles.Core.Enum;

public enum SearchablePages
{
    [Description("Innehållssida")] ContentPage,
    [Description("Listningssida")] ListingPage,
    [Description("Listningssida")] SecondaryListingPage,
    [Description("Startsida")] StartPage
}
