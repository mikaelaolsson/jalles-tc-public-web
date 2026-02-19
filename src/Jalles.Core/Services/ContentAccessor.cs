using Jalles.Core.Contracts;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core;

namespace Jalles.Core.Services;

public class ContentAccessor : IContentAccessor
{
    private readonly IPublishedContentQuery _publishedContentQuery;

    public ContentAccessor(IPublishedContentQuery publishedContentQuery)
    {
        _publishedContentQuery = publishedContentQuery;

    }

    public IReadOnlyList<TChild> GetChildPages<TParent, TChild>() where TParent : IPublishedContent where TChild : IPublishedContent
    {
        var parent = GetLandingPage<TParent>();

        return parent?.Children.OfType<TChild>().ToList() ?? new List<TChild>();
    }

    public TParent? GetLandingPage<TParent>() where TParent : IPublishedContent
    {
        var root = GetRoot();

        return root.Children.OfType<TParent>().FirstOrDefault();
    }

    public IPublishedContent GetRoot()
    {
        return _publishedContentQuery.ContentAtRoot().FirstOrDefault() ?? throw new InvalidOperationException("Could not find root node.");
    }
}