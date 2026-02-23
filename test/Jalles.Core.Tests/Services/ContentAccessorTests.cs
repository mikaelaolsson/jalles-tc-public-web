using Jalles.Core.Models.Content;
using Jalles.Core.Services;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services.Navigation;

namespace Jalles.Core.Tests.Services;

public class ContentAccessorTests
{
    private readonly IPublishedContentQuery _publishedContentQuery;
    private readonly IDocumentNavigationQueryService _documentNavigationQueryService;
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IPublishedContent _root;
    private readonly ContentAccessor _contentAccessor;
    private static readonly Guid _rootKey = Guid.NewGuid();

    public ContentAccessorTests()
    {
        _publishedContentQuery = A.Fake<IPublishedContentQuery>();
        _documentNavigationQueryService = A.Fake<IDocumentNavigationQueryService>();
        _publishedValueFallback = A.Fake<IPublishedValueFallback>();

        _root = A.Fake<IPublishedContent>();

        A.CallTo(() => _root.Key).Returns(_rootKey);

        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([_root]);

        _contentAccessor = new ContentAccessor(
            _publishedContentQuery,
            _documentNavigationQueryService);
    }

    [Fact]
    public void GetAllChildren_ShouldReturnChildren_WhenParentHasChildren()
    {
        // Arrange
        var parent = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var child1 = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var child2 = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var childKeys = new[] { child1.Key, child2.Key }.AsEnumerable();

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out childKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(childKeys);

        A.CallTo(() => _publishedContentQuery.Content(childKeys))
            .Returns([child1, child2]);

        // Act
        var result = _contentAccessor.GetAllChildren(parent);

        // Assert
        result.ShouldBe([child1, child2], ignoreOrder: true);
    }

    [Fact]
    public void GetAllChildren_ShouldReturnEmpty_WhenTryGetChildrenKeysFails()
    {
        // Arrange
        var parent = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);

        IEnumerable<Guid> unused;

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out unused))
            .Returns(false);

        // Act
        var result = _contentAccessor.GetAllChildren(parent);

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetFirstChildOfTypeFromRoot_ShouldReturnFirstMatchingChild()
    {
        // Arrange
        var expected = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var other = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var children = new IPublishedContent[] { other, expected };

        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([_root]);

        var expectedKey = new[] { expected.Key }.AsEnumerable();

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(_root.Key, out expectedKey))
            .Returns(true)
            .AssignsOutAndRefParameters(expectedKey);

        A.CallTo(() => _publishedContentQuery.Content(A<IEnumerable<Guid>>._))
            .Returns(children);

        // Act
        var result = _contentAccessor.GetFirstChildOfTypeFromRoot<ListingPage>();

        // Assert
        result.ShouldBe(expected);
        result.ShouldBeOfType<ListingPage>();
    }

    [Fact]
    public void GetFirstChildOfTypeFromRoot_ShouldReturnNull_WhenNoMatchingChildFound()
    {
        // Arrange
        var child = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);

        var childKeys = new[] { child.Key }.AsEnumerable();

        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([_root]);

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(_root.Key, out childKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(childKeys);

        A.CallTo(() => _publishedContentQuery.Content(childKeys))
            .Returns([child]);

        // Act
        var result = _contentAccessor.GetFirstChildOfTypeFromRoot<StartPage>();

        // Assert
        result.ShouldBeNull(); // because StartPage is not a child of StartPage
    }

    [Fact]
    public void GetChildrenOfType_ShouldReturnOnlyChildrenOfExpectedType_WhenParentHasMultipleChildTypes()
    {
        // Arrange
        var contentPage1 = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var contentPage2 = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var listingPage = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var parentListingPage = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);

        var childKeys = new[] { contentPage1.Key, contentPage2.Key, listingPage.Key }.AsEnumerable();
        var parentKeys = new[] { parentListingPage.Key }.AsEnumerable();

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(parentListingPage.Key, out childKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(childKeys);

        A.CallTo(() => _publishedContentQuery.Content(childKeys))
            .Returns([contentPage1, contentPage2, listingPage]);

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(_root.Key, out parentKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(parentKeys);

        A.CallTo(() => _publishedContentQuery.Content(parentKeys))
            .Returns([parentListingPage]);

        // Act
        var result = _contentAccessor.GetChildrenOfType<ListingPage, ContentPage>();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldBe([contentPage1, contentPage2], ignoreOrder: true);
        result.All(p => p is not null).ShouldBeTrue();
        result.All(p => p is ContentPage).ShouldBeTrue();
    }

    [Fact]
    public void GetChildrenOfType_ShouldReturnChildren_WhenParentIsStartPage()
    {
        // Arrange
        var child1 = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var child2 = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);

        var childKeys = new[] { child1.Key, child2.Key }.AsEnumerable();

        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([_root]);

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(_root.Key, out childKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(childKeys);

        A.CallTo(() => _publishedContentQuery.Content(childKeys))
            .Returns([child1, child2]);

        // Act
        var result = _contentAccessor.GetChildrenOfType<StartPage, ContentPage>();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result.ShouldBe([child1]);
        result.All(p => p is not null).ShouldBeTrue();
        result.All(p => p is ContentPage).ShouldBeTrue();
    }

    [Fact]
    public void GetChildrenOfType_ShouldReturnEmpty_WhenStartPageAsParentHasNoChildrenOfRequestedType()
    {
        // Arrange
        var child = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var childKeys = new[] { child.Key }.AsEnumerable();

        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([_root]);

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(_root.Key, out childKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(childKeys);

        A.CallTo(() => _publishedContentQuery.Content(childKeys))
            .Returns([child]);

        // Act
        var result = _contentAccessor.GetChildrenOfType<StartPage, ContentPage>();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetChildrenOfType_ShouldReturnEmpty_WhenParentTypeDoesNotExistUnderRoot()
    {
        // Arrange
        var listingPage = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var parentKeys = new[] { listingPage.Key }.AsEnumerable();

        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([_root]);

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(_root.Key, out parentKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(parentKeys);

        A.CallTo(() => _publishedContentQuery.Content(parentKeys))
            .Returns([listingPage]);

        // Act
        var result = _contentAccessor.GetChildrenOfType<ListingPage, ContentPage>();

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetParent_ShouldReturnParentOfCorrectType_WhenParentExists()
    {
        // Arrange
        var parent = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var child = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);

        Guid? parentKey = parent.Key;
        A.CallTo(() => _documentNavigationQueryService.TryGetParentKey(child.Key, out parentKey))
            .Returns(true)
            .AssignsOutAndRefParameters(parentKey);

        A.CallTo(() => _publishedContentQuery.Content(parentKey)).Returns(parent);

        // Act
        var result = _contentAccessor.GetParent<ListingPage>(child);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(parent);
        result.ShouldBeOfType<ListingPage>();
    }

    [Fact]
    public void GetParent_ShouldReturnNull_WhenParentDoesNotExist()
    {
        // Arrange
        var child = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        Guid? parentKey = null;

        A.CallTo(() => _documentNavigationQueryService.TryGetParentKey(child.Key, out parentKey))
            .Returns(false)
            .AssignsOutAndRefParameters(parentKey);

        // Act
        var result = _contentAccessor.GetParent<ListingPage>(child);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetParent_ShouldReturnNull_WhenParentIsNotOfRequestedType()
    {
        // Arrange
        var parent = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var child = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);

        Guid? parentKey = parent.Key;
        A.CallTo(() => _documentNavigationQueryService.TryGetParentKey(child.Key, out parentKey))
            .Returns(true)
            .AssignsOutAndRefParameters(parentKey);

        A.CallTo(() => _publishedContentQuery.Content(parentKey)).Returns(parent);

        // Act
        var result = _contentAccessor.GetParent<ContentPage>(child);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetRoot_ShouldReturnRootNode_WhenRootExists()
    {
        // Arrange
        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([_root]);

        // Act
        var result = _contentAccessor.GetRoot();

        // Assert
        result.ShouldBe(_root);
    }

    [Fact]
    public void GetRoot_ShouldThrowInvalidOperationException_WhenNoRootExists()
    {
        // Arrange
        A.CallTo(() => _publishedContentQuery.ContentAtRoot())
            .Returns([]);

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _contentAccessor.GetRoot())
            .Message.ShouldContain("Could not find root node.");
    }

    [Fact]
    public void GetChildrenOfTypeFromParent_ShouldReturnChildrenOfRequestedType()
    {
        // Arrange
        var parent = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var child1 = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var child2 = CreateModel<ContentPage>("contentPage", Guid.NewGuid(), _publishedValueFallback);
        var child3 = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var childKeys = new[] { child1.Key, child2.Key, child3.Key }.AsEnumerable();

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out childKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(childKeys);

        A.CallTo(() => _publishedContentQuery.Content(childKeys))
            .Returns([child1, child2, child3]);

        // Act
        var result = _contentAccessor.GetChildrenOfTypeFromParent<ContentPage>(parent);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldBe([child1, child2], ignoreOrder: true);
        result.All(p => p is ContentPage).ShouldBeTrue();
    }

    [Fact]
    public void GetChildrenOfTypeFromParent_ShouldReturnEmpty_WhenNoChildrenOfRequestedType()
    {
        // Arrange
        var parent = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var child = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        var childKeys = new[] { child.Key }.AsEnumerable();

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out childKeys))
            .Returns(true)
            .AssignsOutAndRefParameters(childKeys);

        A.CallTo(() => _publishedContentQuery.Content(childKeys))
            .Returns([child]);

        // Act
        var result = _contentAccessor.GetChildrenOfTypeFromParent<ContentPage>(parent);

        // Assert
        result.ShouldBeEmpty();
    }

    [Fact]
    public void GetChildrenOfTypeFromParent_ShouldReturnEmpty_WhenTryGetChildrenKeysFails()
    {
        // Arrange
        var parent = CreateModel<ListingPage>("listingPage", Guid.NewGuid(), _publishedValueFallback);
        IEnumerable<Guid> unused;

        A.CallTo(() => _documentNavigationQueryService.TryGetChildrenKeys(parent.Key, out unused))
            .Returns(false);

        // Act
        var result = _contentAccessor.GetChildrenOfTypeFromParent<ContentPage>(parent);

        // Assert
        result.ShouldBeEmpty();
    }

    private static T CreateModel<T>(
        string contentTypeAlias,
        Guid key,
        IPublishedValueFallback fallback)
    where T : IPublishedContent
    {
        var content = A.Fake<IPublishedContent>();
        A.CallTo(() => content.ContentType.Alias).Returns(contentTypeAlias);
        A.CallTo(() => content.Key).Returns(key);


        return (T)Activator.CreateInstance(typeof(T), content, fallback)!;
    }
}
