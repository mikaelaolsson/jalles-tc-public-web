using Microsoft.AspNetCore.Mvc.Rendering;

namespace Jalles.Core.Extensions;

public static class ListingPageExtensions
{
    public static IEnumerable<SelectListItem> GetFilters(this IEnumerable<string>? filters)
    {
        if(filters is null) return new List<SelectListItem>();
        var category = new SelectListGroup { Name = "Category" };

        var categories = filters.Select(c => new SelectListItem { Value = c, Text = c, Group = category });

        return categories;
    }
}
