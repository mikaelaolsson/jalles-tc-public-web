using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services.Navigation;
using Jalles.Core.Contracts;

namespace Jalles.Core.Services;

public class ContentAccessor : IContentAccessor
{
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IDocumentNavigationQueryService _documentNavigationQueryService;

    public ContentAccessor(
        IPublishedContentQuery publishedContentQuery,
        IDocumentNavigationQueryService documentNavigationQueryService)
    {
        _publishedContentQuery = publishedContentQuery;
        _documentNavigationQueryService = documentNavigationQueryService;
    }

    public TParent? GetParent<TParent>(IPublishedContent child)
        where TParent : class, IPublishedContent
    {
        _documentNavigationQueryService.TryGetParentKey(child.Key, out var parentKey);

        if(parentKey == null)
            return null;

        return _publishedContentQuery.Content(parentKey) as TParent;
    }

    public IEnumerable<IPublishedContent> GetAllChildren(IPublishedContent parent)
    {
        return !_documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out var childKeys)
            ? []
            : _publishedContentQuery
                .Content(childKeys);
    }

    public IReadOnlyList<TChild> GetChildrenOfType<TParent, TChild>()
        where TParent : IPublishedContent
        where TChild : IPublishedContent
    {
        var parent = typeof(TParent) == typeof(StartPage)
            ? GetRoot()
            : GetFirstChildOfTypeFromRoot<TParent>();

        if(parent == null)
            return [];

        if(!_documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out var childKeys))
            return [];

        return _publishedContentQuery
            .Content(childKeys)
            .OfType<TChild>()
            .ToList();
    }

    public IReadOnlyList<TChild> GetChildrenOfTypeFromParent<TChild>(IPublishedContent parent)
        where TChild : IPublishedContent
    {
        if(!_documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out var childKeys))
            return [];

        return _publishedContentQuery
            .Content(childKeys)
            .OfType<TChild>()
            .ToList();
    }

    public IPublishedContent GetRoot()
    {
        return _publishedContentQuery
            .ContentAtRoot()
            .FirstOrDefault() ??
                throw new InvalidOperationException("Could not find root node.");
    }

    internal T? GetFirstChildOfTypeFromRoot<T>() where T : IPublishedContent
    {
        var root = GetRoot();

        _documentNavigationQueryService.TryGetChildrenKeys(root.Key, out var childKeys);

        var poop = _publishedContentQuery
            .Content(childKeys)
            .OfType<T>();

        return poop
            .FirstOrDefault();
    }
}
