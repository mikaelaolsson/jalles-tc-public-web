using System.Globalization;
using AutoMapper;
using Jalles.Core.Contracts;
using Jalles.Core.Services;
using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Jalles.Core.Models.Content;
using Jalles.Core.Constants;

namespace Jalles.Core.Tests.Services;

public class LayoutViewModelServiceTests
{
    private readonly LayoutViewModelService _layoutViewModelService;
    private readonly IContentAccessor _contentAccessor;
    private readonly IUmbracoPagePathService _umbracoPagePathService;
    private readonly IMapper _mapper;

    public LayoutViewModelServiceTests()
    {
        _contentAccessor = A.Fake<IContentAccessor>();
        _umbracoPagePathService = A.Fake<IUmbracoPagePathService>();
        _mapper = A.Fake<IMapper>();

        _layoutViewModelService = new LayoutViewModelService(_umbracoPagePathService, _contentAccessor, _mapper);
    }

    [Fact]
    public void GetTitle_ReturnsTitleProperty_WhenPresent()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => content.GetProperty("title")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns("My Custom Title");

        // Act
        var result = _layoutViewModelService.GetTitle(content);

        // Assert
        result.ShouldBe("My Custom Title");
    }

    [Fact]
    public void GetTitle_ReturnsEmptyString_WhenContentIsNull()
    {
        // Act
        var result = _layoutViewModelService.GetTitle(null);

        // Assert
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void GetTitle_ReturnsHeadingProperty_WhenTitleIsNullOrWhitespace_AndHeadingIsPresent()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var titleProperty = A.Fake<IPublishedProperty>();
        var headingProperty = A.Fake<IPublishedProperty>();

        A.CallTo(() => content.GetProperty("title")).Returns(titleProperty);
        A.CallTo(() => titleProperty.GetValue(null, null)).Returns("   ");
        A.CallTo(() => content.GetProperty("heading")).Returns(headingProperty);
        A.CallTo(() => headingProperty.GetValue(null, null)).Returns("My Heading");

        // Act
        var result = _layoutViewModelService.GetTitle(content);

        // Assert
        result.ShouldBe("My Heading");
    }

    [Fact]
    public void GetTitle_ReturnsContentName_WhenTitleAndHeadingAreNullOrWhitespace()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var titleProperty = A.Fake<IPublishedProperty>();
        var headingProperty = A.Fake<IPublishedProperty>();

        A.CallTo(() => content.GetProperty("title")).Returns(titleProperty);
        A.CallTo(() => titleProperty.GetValue(null, null)).Returns("   ");
        A.CallTo(() => content.GetProperty("heading")).Returns(headingProperty);
        A.CallTo(() => headingProperty.GetValue(null, null)).Returns("");
        A.CallTo(() => content.Name).Returns("Fallback Name");

        // Act
        var result = _layoutViewModelService.GetTitle(content);

        // Assert
        result.ShouldBe("Fallback Name");
    }

    [Fact]
    public void GetMetaDescription_ReturnsMetaDescription_WhenPresent()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var property = A.Fake<IPublishedProperty>();

        A.CallTo(() => content.GetProperty("metaDescription")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns("Meta Description");

        // Act
        var result = _layoutViewModelService.GetMetaDescription(content);

        // Assert
        result.ShouldBe("Meta Description");
    }

    [Fact]
    public void GetMetaDescription_ReturnsEmptyString_WhenContentIsNull()
    {
        // Arrange & Act
        var result = _layoutViewModelService.GetMetaDescription(null);

        // Assert
        result.ShouldBe(string.Empty);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetMetaDescription_ReturnsParentMetaDescription_WhenCurrentMetaDescriptionIsNull(string currentMetaDescription)
    {
        // Arrange
        // Current content with no MetaDescription
        var content = A.Fake<IPublishedContent>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => content.GetProperty("metaDescription")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(currentMetaDescription);

        // Parent content with MetaDescription
        var parent = A.Fake<IPublishedContent>();
        var parentProperty = A.Fake<IPublishedProperty>();
        A.CallTo(() => parent.GetProperty("metaDescription")).Returns(parentProperty);
        A.CallTo(() => parentProperty.GetValue(null, null)).Returns("Parent Meta Description");

        A.CallTo(() => _contentAccessor.GetParent<IPublishedContent>(content)).Returns(parent);

        // Act
        var result = _layoutViewModelService.GetMetaDescription(content);

        // Assert
        result.ShouldBe("Parent Meta Description");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetMetaDescription_ReturnsEmptyString_WhenNoMetaDescriptionInContentOrParents(string currentMetaDescription)
    {
        // Arrange
        // Current content with no MetaDescription
        var content = A.Fake<IPublishedContent>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => content.GetProperty("metaDescription")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(currentMetaDescription);

        // Parent content with no MetaDescription
        var parent = A.Fake<IPublishedContent>();
        var parentProperty = A.Fake<IPublishedProperty>();
        A.CallTo(() => parent.GetProperty("metaDescription")).Returns(parentProperty);
        A.CallTo(() => parentProperty.GetValue(null, null)).Returns(currentMetaDescription);

        A.CallTo(() => _contentAccessor.GetParent<IPublishedContent>(content)).Returns(parent);
        A.CallTo(() => _contentAccessor.GetParent<IPublishedContent>(parent)).Returns(null);

        // Act
        var result = _layoutViewModelService.GetMetaDescription(content);

        // Assert
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void GetUrl_ReturnsBaseUrl_WhenContentIsNull()
    {
        // Arrange
        var context = A.Fake<HttpContext>();
        A.CallTo(() => context.Request.Host).Returns(new HostString("site.com"));

        // Act
        var result = _layoutViewModelService.GetUrl(null, context);

        // Assert
        result.ShouldBe("https://site.com");
    }

    [Fact]
    public void GetUrl_ReturnsPageUrl_WhenContentIsNotNull()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var context = A.Fake<HttpContext>();
        var expectedUri = new Uri("https://site.com/page");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(content)).Returns(expectedUri);

        // Act
        var result = _layoutViewModelService.GetUrl(content, context);

        // Assert
        result.ShouldBe(expectedUri.ToString());
    }

    [Fact]
    public void GetThumbnail_ReturnsDefaultThumbnail_WhenNoThumbnailProperty()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var context = A.Fake<HttpContext>();
        A.CallTo(() => context.Request.Host).Returns(new HostString("site.com"));

        A.CallTo(() => content.GetProperty("thumbnail")).Returns(null);

        // Act
        var result = _layoutViewModelService.GetThumbnail(content, context);

        // Assert
        result.ShouldBe($"https://site.com{JallesConstants.DefaultFallbackThumbnail}");
    }

    [Fact]
    public void GetThumbnail_ReturnsDefaultThumbnail_WhenThumbnailPropertyIsNull()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var context = A.Fake<HttpContext>();
        A.CallTo(() => context.Request.Host).Returns(new HostString("site.com"));

        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => content.GetProperty("thumbnail")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(null);

        // Act
        var result = _layoutViewModelService.GetThumbnail(content, context);

        // Assert
        result.ShouldBe($"https://site.com{JallesConstants.DefaultFallbackThumbnail}");
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void GetThumbnail_ReturnsDefaultThumbnail_WhenCropUrlIsEmptyOrWhitespace(string cropUrl)
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var context = A.Fake<HttpContext>();
        A.CallTo(() => context.Request.Host).Returns(new HostString("site.com"));

        var property = A.Fake<IPublishedProperty>();
        var mediaWithCrops = A.Fake<MediaWithCrops>();
        A.CallTo(() => content.GetProperty("thumbnail")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(mediaWithCrops);

        A.CallTo(() => _umbracoPagePathService.GetCropUrl(mediaWithCrops, "thumbnail")).Returns(cropUrl);

        // Act
        var result = _layoutViewModelService.GetThumbnail(content, context);

        // Assert
        result.ShouldBe($"https://site.com{JallesConstants.DefaultFallbackThumbnail}");
    }

    [Fact]
    public void GetThumbnail_ReturnsCropUrl_WhenCropUrlIsValid()
    {
        // Arrange
        var content = A.Fake<IPublishedContent>();
        var context = A.Fake<HttpContext>();
        A.CallTo(() => context.Request.Host).Returns(new HostString("site.com"));

        var property = A.Fake<IPublishedProperty>();
        var mediaWithCrops = A.Fake<MediaWithCrops>();
        A.CallTo(() => content.GetProperty("thumbnail")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(mediaWithCrops);

        A.CallTo(() => _umbracoPagePathService.GetCropUrl(mediaWithCrops, "thumbnail"))
            .Returns("/media/abc.jpg?width=1200&height=627");

        // Act
        var result = _layoutViewModelService.GetThumbnail(content, context);

        // Assert
        result.ShouldBe("https://site.com/media/abc.jpg?width=1200&height=627");
        Uri.TryCreate(result, UriKind.Absolute, out var uri).ShouldBeTrue();
        Uri.IsWellFormedUriString(result, UriKind.Absolute).ShouldBeTrue();
    }


    [Fact]
    public void BuildHeader_ReturnsHeaderBlockViewModel_WhenHeaderBlockExists()
    {
        // Arrange
        var culture = new CultureInfo("sv-SE");
        var content = A.Fake<IPublishedContent>();
        var headerBlock = A.Fake<HeaderBlock>();
        var logo = A.Fake<MediaWithCrops>();
        var ctaBlockList = A.Fake<BlockListModel>();
        var media = A.Fake<MediaWithCrops>();

        // Create a real BlockListItem instead of faking it
        var blockListItem = new BlockListItem<HeaderBlock>(Guid.NewGuid(), headerBlock, null, null);

        // Setup the header property to return the BlockListItem
        var headerProperty = A.Fake<IPublishedProperty>();
        A.CallTo(() => content.GetProperty("header")).Returns(headerProperty);
        A.CallTo(() => headerProperty.GetValue(null, null)).Returns(blockListItem);

        // Setup HeaderBlock properties
        A.CallTo(() => headerBlock.Media).Returns(media);
        A.CallTo(() => headerBlock.BackgroundColor).Returns(new ColorPickerValueConverter.PickedColor("#FFEB19", "Jalles Yellow"));

        // Setup page type
        var contentTypeAlias = A.Fake<IPublishedContentType>();
        A.CallTo(() => content.ContentType).Returns(contentTypeAlias);
        A.CallTo(() => contentTypeAlias.Alias).Returns("standardPage");

        A.CallTo(() => _umbracoPagePathService.GetMediaUrl(media, culture)).Returns("/media/test.jpg");

        // Act
        var result = _layoutViewModelService.BuildHeader(content, culture);

        // Assert
        result.ShouldNotBeNull();
        result.MediaBlock.ShouldNotBeNull();
        result.MediaBlock.Media.ShouldBe(media);
        result.MediaBlock.MediaSource.ShouldBe("/media/test.jpg");
        result.MediaBlock.BackgroundColor.ShouldBe("#FFEB19");
        result.MediaBlock.IsLazy.ShouldBe(false);
    }

    [Fact]
    public void BuildHeader_ReturnsEmptyHeaderBlockViewModel_WhenContentIsNull()
    {
        // Arrange
        var culture = new CultureInfo("");

        // Act
        var result = _layoutViewModelService.BuildHeader(null, culture);

        // Assert
        result.ShouldNotBeNull();
        result.MediaBlock.ShouldNotBeNull();
        result.MediaBlock.MediaSource.ShouldBe(JallesConstants.DefaultFallbackMedia);
    }

    [Fact]
    public void BuildHeader_ReturnsEmptyHeaderBlockViewModel_WhenNoHeaderPropertyExists()
    {
        // Arrange
        var culture = new CultureInfo("sv-SE");
        var content = A.Fake<IPublishedContent>();

        // No header property
        A.CallTo(() => content.GetProperty("header")).Returns(null);

        // Act
        var result = _layoutViewModelService.BuildHeader(content, culture);

        // Assert
        result.ShouldNotBeNull();
        result.MediaBlock.ShouldNotBeNull();
        result.MediaBlock.MediaSource.ShouldBe(JallesConstants.DefaultFallbackMedia);
    }
}
