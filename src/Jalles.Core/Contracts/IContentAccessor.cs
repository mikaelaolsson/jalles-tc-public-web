namespace Jalles.Core.Contracts;

public interface IContentAccessor
{
    TParent? GetParent<TParent>(IPublishedContent child) where TParent : class, IPublishedContent;
    IEnumerable<IPublishedContent> GetAllChildren(IPublishedContent parent);
    IReadOnlyList<TChild> GetChildrenOfType<TParent, TChild>() where TParent : IPublishedContent where TChild : IPublishedContent;
    IReadOnlyList<TChild> GetChildrenOfTypeFromParent<TChild>(IPublishedContent parent) where TChild : IPublishedContent;
    IPublishedContent GetRoot();
}
