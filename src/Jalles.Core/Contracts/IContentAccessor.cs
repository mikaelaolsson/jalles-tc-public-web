using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Core.Contracts;

public interface IContentAccessor
{
    IReadOnlyList<TChild> GetChildPages<TParent, TChild>() where TParent : IPublishedContent where TChild : IPublishedContent;
    TParent? GetLandingPage<TParent>() where TParent : IPublishedContent;
    IPublishedContent GetRoot();
}