using System.Globalization;
using Jalles.Core.Services;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services.Navigation;
using static Umbraco.Cms.Core.PropertyEditors.ValueConverters.ImageCropperValue;

namespace Jalles.Core.Tests.Services;

public class UmbracoPagePathServiceTests
{
    private readonly IPublishedUrlProvider _publishedUrlProvider = A.Fake<IPublishedUrlProvider>();
    private readonly IImageUrlGenerator _imageUrlGenerator = A.Fake<IImageUrlGenerator>();
    private readonly IDocumentNavigationQueryService _documentNavigationQueryService = A.Fake<IDocumentNavigationQueryService>();
    private readonly IPublishedContentQuery _publishedContentQuery = A.Fake<IPublishedContentQuery>();

    private readonly UmbracoPagePathService _umbracoPagePathService;

    public UmbracoPagePathServiceTests()
    {
        _umbracoPagePathService = new UmbracoPagePathService(
            _publishedUrlProvider,
            _imageUrlGenerator,
            _documentNavigationQueryService,
            _publishedContentQuery);
    }

    [Fact]
    public void GetPageUri_ShouldReturnAbsoluteUri()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        A.CallTo(() => _publishedUrlProvider.GetUrl(content, UrlMode.Absolute, null, null)).Returns("https://site.com/path");

        // Act
        var result = _umbracoPagePathService.GetPageUri(content);

        // Assert
        result.ShouldBe(new Uri("https://site.com/path"));
    }

    [Fact]
    public void GetPagePath_ShouldReturnSlash_WhenContentIsNull()
    {
        // Act
        var result = _umbracoPagePathService.GetPagePath(null);

        // Assert
        result.ShouldBe("/");
    }

    [Fact]
    public void GetPagePath_ShouldReturnSlash_WhenQueryFails()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        A.CallTo(() => content.Key).Returns(Guid.NewGuid());

        var dummyOut = Enumerable.Empty<Guid>();
        A.CallTo(() => _documentNavigationQueryService.TryGetAncestorsOrSelfKeys(content.Key, out dummyOut))
            .WithAnyArguments()
            .Returns(false);

        // Act
        var result = _umbracoPagePathService.GetPagePath(content);

        // Assert
        result.ShouldBe("/");
    }

    [Fact]
    public void GetPagePath_ShouldReturnCorrectPathIncludingSelf()
    {
        // Arrange
        var contentPageKey = Guid.NewGuid();
        var listingPageKey = Guid.NewGuid();
        var fakeContentKey = Guid.NewGuid();

        var fakeContent = A.Fake<IPublishedContent>();
        A.CallTo(() => fakeContent.Key).Returns(fakeContentKey);

        var ancestorKeys = new List<Guid> { fakeContentKey, contentPageKey, listingPageKey }.AsEnumerable();
        A.CallTo(() => _documentNavigationQueryService.TryGetAncestorsOrSelfKeys(fakeContent.Key, out ancestorKeys))
            .Returns(true);

        var fakePage = A.Fake<IPublishedContent>();
        var contentPage = A.Fake<IPublishedContent>();
        var listingPage = A.Fake<IPublishedContent>();

        A.CallTo(() => _publishedContentQuery.Content(fakeContentKey)).Returns(fakePage);
        A.CallTo(() => _publishedContentQuery.Content(contentPageKey)).Returns(contentPage);
        A.CallTo(() => _publishedContentQuery.Content(listingPageKey)).Returns(listingPage);

        A.CallTo(() => contentPage.ContentType.Alias).Returns("contentPage");
        A.CallTo(() => listingPage.ContentType.Alias).Returns("listingPage");

        A.CallTo(() => fakePage.UrlSegment).Returns("fakecontent");
        A.CallTo(() => contentPage.UrlSegment).Returns("contentpage");
        A.CallTo(() => listingPage.UrlSegment).Returns("listingpage");

        // Act
        var result = _umbracoPagePathService.GetPagePath(fakeContent);

        // Assert
        result.ShouldBe("/listingpage/contentpage/fakecontent/");
    }

    [Fact]
    public void GetParentPagePath_ShouldExcludeStartPage()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var guid = Guid.NewGuid();
        A.CallTo(() => content.Key).Returns(Guid.NewGuid());

        var keys = new List<Guid> { guid }.AsEnumerable();
        A.CallTo(() => _documentNavigationQueryService.TryGetAncestorsKeys(content.Key, out keys))
            .Returns(true);

        var startPage = A.Fake<IPublishedContent>();
        A.CallTo(() => _publishedContentQuery.Content(guid)).Returns(startPage);
        A.CallTo(() => startPage.ContentType.Alias).Returns("startPage");

        // Act
        var result = _umbracoPagePathService.GetParentPagePath(content);

        // Assert
        result.ShouldBe("/");
    }

    [Fact]
    public void GetParentPagePath_ShouldReturnCorrectParentPath()
    {
        // Arrange
        var contentPageKey = Guid.NewGuid();
        var listingPageKey = Guid.NewGuid();
        var fakeContentKey = Guid.NewGuid();

        var fakeContent = A.Fake<IPublishedContent>();
        A.CallTo(() => fakeContent.Key).Returns(fakeContentKey);

        var ancestorKeys = new List<Guid> { contentPageKey, listingPageKey }.AsEnumerable();
        A.CallTo(() => _documentNavigationQueryService.TryGetAncestorsKeys(fakeContent.Key, out ancestorKeys))
            .Returns(true);

        var contentPage = A.Fake<IPublishedContent>();
        var listingPage = A.Fake<IPublishedContent>();

        A.CallTo(() => _publishedContentQuery.Content(contentPageKey)).Returns(contentPage);
        A.CallTo(() => _publishedContentQuery.Content(listingPageKey)).Returns(listingPage);

        A.CallTo(() => contentPage.ContentType.Alias).Returns("contentPage");
        A.CallTo(() => listingPage.ContentType.Alias).Returns("listingPage");

        A.CallTo(() => contentPage.UrlSegment).Returns("contentpage");
        A.CallTo(() => listingPage.UrlSegment).Returns("listingpage");

        // Act
        var result = _umbracoPagePathService.GetParentPagePath(fakeContent);

        // Assert
        result.ShouldBe("/listingpage/contentpage/");
    }

    [Fact]
    public void GetMediaUrl_ShouldReturnEmptyString_WhenMediaIsNull()
    {
        // Arrange
        var culture = new CultureInfo("en-US");

        // Act
        var result = _umbracoPagePathService.GetMediaUrl(null, culture);

        // Assert
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void GetMediaUrl_ShouldReturnEmptyString_WhenProviderReturnsEmptyString()
    {
        // Arrange
        var media = A.Fake<IPublishedContent>();
        var culture = new CultureInfo("en-US");
        A.CallTo(() => _publishedUrlProvider.GetMediaUrl(
            media,
            UrlMode.Default,
            culture.Name,
            Umbraco.Cms.Core.Constants.Conventions.Media.File,
            null))
            .Returns(string.Empty);

        // Act
        var result = _umbracoPagePathService.GetMediaUrl(media, culture);

        // Assert
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void GetMediaUrl_ShouldReturnUrl_WhenMediaIsNotNull()
    {
        // Arrange
        var media = A.Fake<MediaWithCrops>();
        var culture = new CultureInfo("sv-SE");
        const string expectedPath = "/media/ylxl3raz/6f5b3393-2-1.jpg";

        A.CallTo(() => _publishedUrlProvider.GetMediaUrl(
            media,
            UrlMode.Default,
            culture.Name,
            Umbraco.Cms.Core.Constants.Conventions.Media.File,
            null))
            .Returns(expectedPath);

        // Act
        var result = _umbracoPagePathService.GetMediaUrl(media, culture);

        // Assert
        result.ShouldBe(expectedPath);
    }

    [Fact]
    public void GetCropUrl_ShouldReturnEmptyString_WhenMediaIsNull()
    {
        // Act
        var result = _umbracoPagePathService.GetCropUrl(null, "cropAlias");

        // Assert
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void GetCropUrl_ReturnsCroppedUrl_WhenCropExists()
    {
        // Arrange
        var crop = new ImageCropperCrop { Alias = "cropAlias", Width = 1200, Height = 627 };
        var crops = new List<ImageCropperCrop> { crop };
        var localCrops = new ImageCropperValue { Crops = crops, Src = "/media/013pnoyz/6f5b1973-5.jpg" };

        var content = A.Fake<IPublishedContent>();
        var publishedValueFallback = A.Fake<IPublishedValueFallback>();
        var media = new MediaWithCrops(content, publishedValueFallback, localCrops);

        A.CallTo(() => _imageUrlGenerator.GetImageUrl(
            A<ImageUrlGenerationOptions>.That.Matches(o =>
                o.ImageUrl == "/media/013pnoyz/6f5b1973-5.jpg" &&
                o.Width == 1200 &&
                o.Height == 627)))
            .Returns("/media/013pnoyz/6f5b1973-5.jpg?width=1200&height=627");

        // Act
        var result = _umbracoPagePathService.GetCropUrl(media, "cropAlias");

        // Assert
        result.ShouldBe("/media/013pnoyz/6f5b1973-5.jpg?width=1200&height=627");
    }

    [Fact]
    public void GetCropUrl_ReturnsOriginalSrc_WhenCropIsNull()
    {
        // Arrange
        var localCrops = new ImageCropperValue { Crops = null, Src = "/media/013pnoyz/6f5b1973-5.jpg" };
        var content = A.Fake<IPublishedContent>();
        var publishedValueFallback = A.Fake<IPublishedValueFallback>();
        var media = new MediaWithCrops(content, publishedValueFallback, localCrops);

        // Act
        var result = _umbracoPagePathService.GetCropUrl(media, "anyCropAlias");

        // Assert
        result.ShouldBe("/media/013pnoyz/6f5b1973-5.jpg");
    }
}
