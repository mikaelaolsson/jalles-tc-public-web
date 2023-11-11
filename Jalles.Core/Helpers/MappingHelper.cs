using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Core.Helpers;

public static class MappingHelper
{
    public static string GetParentPagePath(this IPublishedContent? child)
    {
        var pages = child?.Ancestors().ToArray();

        return pages is null or [{ ContentType.Alias: "startPage" }] ? "/" :
            pages.Reverse()
                .Where(page => page.ContentType.Alias != "startPage")
                .Aggregate("", (current, page) => current + $"/{page.UrlSegment}");
    }

    public static string GetPagePath(this IPublishedContent? child)
    {
        var pages = child?.AncestorsOrSelf().ToArray();

        return pages is null or [{ ContentType.Alias: "startPage" }] ? "/" :
            pages.Reverse()
                .Where(page => page.ContentType.Alias != "startPage")
                .Aggregate("", (current, page) => current + $"/{page.UrlSegment}");
    }
}