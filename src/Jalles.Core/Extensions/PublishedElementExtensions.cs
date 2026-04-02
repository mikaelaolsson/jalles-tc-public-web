using Umbraco.Cms.Core.Models.Blocks;
using Jalles.Core.Contracts;

namespace Jalles.Core.Extensions;

public static class PublishedElementExtensions
{
    public static string GetString(this IPublishedElement element, string alias)
    {
        var prop = element?.GetProperty(alias);
        return prop?.GetValue() as string ?? string.Empty;
    }

    public static string GetString(this IPublishedContent? content, string alias, IContentAccessor contentAccessor)
    {
        if(content == null)
            return string.Empty;

        var current = content;
        while(current != null)
        {
            var value = GetString(current, alias);
            if(!string.IsNullOrWhiteSpace(value))
                return value;

            current = contentAccessor.GetParent<IPublishedContent>(current);
        }

        return string.Empty;
    }

    public static bool GetBool(this IPublishedElement element, string alias)
    {
        var prop = element?.GetProperty(alias);
        var value = prop?.GetValue();
        return value is bool b ? b : (value is string s && bool.TryParse(s, out var result) && result);
    }

    public static MediaWithCrops? GetMediaWithCrops(this IPublishedElement element, string alias)
    {
        var prop = element?.GetProperty(alias);
        return prop?.GetValue() as MediaWithCrops;
    }

    public static BlockListItem<T>? GetBlockListItem<T>(this IPublishedElement element, string alias)
        where T : IPublishedElement
    {
        var prop = element?.GetProperty(alias);
        return prop?.GetValue() as BlockListItem<T>;
    }
}
