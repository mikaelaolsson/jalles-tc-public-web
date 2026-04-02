using Umbraco.Cms.Core.Models.Blocks;

namespace Jalles.Core.Extensions;

public static class BlockElementExtensions
{
    public static T? GetElementByContentTypeAlias<T>(this IEnumerable<BlockListItem>? blockItems, string contentTypeAlias)
        where T : class, IPublishedElement
    {
        var blockListItems = blockItems?.ToArray() ?? [];

        if(blockItems == null || !blockListItems.Any())
        {
            return null;
        }

        var block = blockListItems.FirstOrDefault(item => item.Content.ContentType.Alias == contentTypeAlias);

        return block?.GetElement<T>();
    }

    public static IEnumerable<T?> GetElementsByContentTypeAlias<T>(this IEnumerable<BlockListItem>? blockItems, string contentTypeAlias)
        where T : class, IPublishedElement
    {
        var blockListItems = blockItems?.ToArray() ?? [];

        if(blockItems == null || !blockListItems.Any())
        {
            return [];
        }

        var blocks = blockListItems.Where(item => item.Content.ContentType.Alias == contentTypeAlias).Select(item => item.GetElement<T>());

        return blocks;
    }

    public static IEnumerable<T> GetElements<T>(this BlockListModel? model)
        where T : IPublishedElement
    {
        if(model is null)
            return [];

        return model
            .Select(x => x.Content)
            .OfType<T>();
    }

    public static IEnumerable<T> GetElements<T>(this IEnumerable<IPublishedContent>? model)
        where T : IPublishedElement
    {
        return model?.OfType<T>() ?? [];
    }

    public static T? GetElement<T>(this BlockListItem? item)
        where T : class, IPublishedElement
    {
        return item?.Content as T;
    }

    public static T? GetElement<T>(this MediaWithCrops? media)
        where T : class, IPublishedElement
    {
        return media?.Content as T;
    }

    public static IEnumerable<T> GetElements<T>(this IEnumerable<MediaWithCrops?>? media)
        where T : IPublishedElement
    {
        return media?.Select(x => x?.Content).OfType<T>() ?? [];
    }
}
