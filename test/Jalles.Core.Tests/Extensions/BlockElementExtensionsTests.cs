using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace Jalles.Core.Tests.Extensions;

public class BlockElementExtensionsTests
{
    [Fact]
    public void GetElementByContentTypeAlias_ShouldReturnCorrectElement_WhenContentTypeAliasMatches()
    {
        // Arrange
        var validContent = A.Fake<MediaBlock>();
        var wrongTypeContent = A.Fake<FooterBlock>();

        var guid = Guid.NewGuid();
        A.CallTo(() => validContent.ContentType.Alias).Returns("mediaBlock");
        A.CallTo(() => wrongTypeContent.ContentType.Alias).Returns("notAMediaBlock");

        var validBlock = new BlockListItem(guid, validContent, guid, validContent);
        var wrongTypeBlock = new BlockListItem(guid, wrongTypeContent, guid, wrongTypeContent);

        var blockItems = new[] { validBlock, wrongTypeBlock };

        // Act
        var result = blockItems.GetElementByContentTypeAlias<MediaBlock>("mediaBlock");

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(validContent);
    }

    [Fact]
    public void GetElementByContentTypeAlias_ShouldReturnNull_WhenContentTypeAliasDoesNotMatch()
    {
        // Arrange
        var validContent = A.Fake<MediaBlock>();
        var wrongTypeContent = A.Fake<FooterBlock>();

        A.CallTo(() => validContent.ContentType.Alias).Returns("mediaBlock");
        A.CallTo(() => wrongTypeContent.ContentType.Alias).Returns("footerBlock");

        var guid = Guid.NewGuid();

        var validBlock = new BlockListItem(guid, validContent, guid, validContent);
        var wrongTypeBlock = new BlockListItem(guid, wrongTypeContent, guid, wrongTypeContent);

        var blockItems = new[] { validBlock, wrongTypeBlock };

        // Act
        var result = blockItems.GetElementByContentTypeAlias<MediaBlock>("nonExistentContentType");

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetElementsByContentTypeAlias_ShouldReturnCorrectElements_WhenContentTypeAliasMatches()
    {
        // Arrange
        var validContent1 = A.Fake<MediaBlock>();
        var validContent2 = A.Fake<MediaBlock>();
        var wrongTypeContent = A.Fake<FooterBlock>();

        A.CallTo(() => validContent1.ContentType.Alias).Returns("mediaBlock");
        A.CallTo(() => validContent2.ContentType.Alias).Returns("mediaBlock");
        A.CallTo(() => wrongTypeContent.ContentType.Alias).Returns("footerBlock");

        var guid = Guid.NewGuid();

        var validBlock1 = new BlockListItem(guid, validContent1, guid, validContent1);
        var validBlock2 = new BlockListItem(guid, validContent2, guid, validContent2);
        var wrongTypeBlock = new BlockListItem(guid, wrongTypeContent, guid, wrongTypeContent);

        var blockItems = new[] { validBlock1, validBlock2, wrongTypeBlock };

        // Act
        var result = blockItems.GetElementsByContentTypeAlias<MediaBlock>("mediaBlock").ToList();

        // Assert
        result.Count.ShouldBe(2);
        result.ShouldContain(validContent1);
        result.ShouldContain(validContent2);
        result.ShouldNotContain(item => item != null && item.GetType() == typeof(FooterBlock));
    }

    [Fact]
    public void GetElementsByContentTypeAlias_ShouldReturnEmpty_WhenNoContentTypeAliasMatches()
    {
        // Arrange
        var validContent = A.Fake<MediaBlock>();
        var wrongTypeContent = A.Fake<FooterBlock>();

        A.CallTo(() => validContent.ContentType.Alias).Returns("mediaBlock");
        A.CallTo(() => wrongTypeContent.ContentType.Alias).Returns("footerBlock");

        var guid = Guid.NewGuid();

        var validBlock = new BlockListItem(guid, validContent, guid, validContent);
        var wrongTypeBlock = new BlockListItem(guid, wrongTypeContent, guid, wrongTypeContent);

        var blockItems = new[] { validBlock, wrongTypeBlock };

        // Act
        var result = blockItems.GetElementsByContentTypeAlias<MediaBlock>("nonExistentContentType").ToList();

        // Assert
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void GetElements_FromBlockListModel_ShouldReturnCorrectModel_WhenCorrectTypeExists()
    {
        // Arrange
        var validContent = A.Fake<MediaBlock>();
        var wrongTypeContent = A.Fake<FooterBlock>();

        var guid = Guid.NewGuid();

        var validBlock = new BlockListItem(guid, validContent, guid, validContent);
        var wrongTypeBlock = new BlockListItem(guid, wrongTypeContent, guid, wrongTypeContent);

        var model = new BlockListModel([validBlock, wrongTypeBlock]);

        // Act
        var result = model.GetElements<MediaBlock>().ToList();

        // Assert
        result.Count.ShouldBe(1);
        result.ShouldContain(validContent);
        result.ShouldNotContain(item => item.GetType() == typeof(FooterBlock));
    }

    [Fact]
    public void GetElements_FromBlockListModel_ShouldReturnEmpty_WhenNoCorrectTypeExists()
    {
        // Arrange
        var validContent = A.Fake<FooterBlock>();

        var guid = Guid.NewGuid();
        var validBlock = new BlockListItem(guid, validContent, guid, validContent);

        var model = new BlockListModel([validBlock]);

        // Act
        var result = model.GetElements<MediaBlock>().ToList();

        // Assert
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void GetElements_FromIPublishedContents_ShouldReturnCorrectModel_WhenCorrectTypeExistsInEnumerable()
    {
        // Arrange
        var validContent = A.Fake<ContentPage>();
        var wrongTypeContent = A.Fake<ListingPage>();

        var model = new[] { validContent, (IPublishedContent)wrongTypeContent };

        // Act
        var result = model.GetElements<ContentPage>().ToList();

        // Assert
        result.Count.ShouldBe(1);
        result.ShouldContain(validContent);
        result.ShouldNotContain(item => item.GetType() == typeof(ListingPage));
    }

    [Fact]
    public void GetElements_FromIPublishedContents_ShouldReturnEmpty_WhenNoCorrectTypeExistsInEnumerable()
    {
        // Arrange
        var validContent = A.Fake<ListingPage>();

        var model = new[] { validContent };

        // Act
        var result = model.GetElements<ContentPage>().ToList();

        // Assert
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void GetElement_FromBlockListItem_ShouldReturnCorrectElement_WhenTypeMatches()
    {
        // Arrange
        var validContent = A.Fake<MediaBlock>();
        var guid = Guid.NewGuid();
        var blockListItem = new BlockListItem(guid, validContent, guid, validContent);

        // Act
        var result = blockListItem.GetElement<MediaBlock>();

        // Assert
        result.ShouldBe(validContent);
    }

    [Fact]
    public void GetElement_FromBlockListItem_ShouldReturnNull_WhenTypeDoesNotMatch()
    {
        // Arrange
        var validContent = A.Fake<MediaBlock>();
        var guid = Guid.NewGuid();
        var blockListItem = new BlockListItem(guid, validContent, guid, validContent);

        // Act
        var result = blockListItem.GetElement<FooterBlock>();

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetElement_FromMediaWithCrops_ShouldReturnCorrectElement_WhenTypeMatches()
    {
        // Arrange
        var validContent = A.Fake<Sponsor>();
        var fallback = A.Fake<IPublishedValueFallback>();
        var crops = new ImageCropperValue();

        var mediaWithCrops = new MediaWithCrops(validContent, fallback, crops);

        // Act
        var result = mediaWithCrops.GetElement<Sponsor>();

        // Assert
        result.ShouldBe(validContent);
    }

    [Fact]
    public void GetElement_FromMediaWithCrops_ShouldReturnNull_WhenTypeDoesNotMatch()
    {
        // Arrange
        var wrongContent = A.Fake<Sponsor>();
        var fallback = A.Fake<IPublishedValueFallback>();
        var crops = new ImageCropperValue();

        var mediaWithCrops = new MediaWithCrops(wrongContent, fallback, crops);

        // Act
        var result = mediaWithCrops.GetElement<Models.Content.File>();

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void GetElements_FromMultipleMediaWithCrops_ShouldReturnOnlyMatchingTypes()
    {
        // Arrange
        var content1 = A.Fake<Sponsor>();
        var content2 = A.Fake<Models.Content.File>();
        var fallback = A.Fake<IPublishedValueFallback>();
        var crops = new ImageCropperValue();

        var media1 = new MediaWithCrops(content1, fallback, crops);
        var media2 = new MediaWithCrops(content2, fallback, crops);

        var mediaList = new[] { media1, media2 };

        // Act
        var result = mediaList.GetElements<Sponsor>().ToList();

        // Assert
        result.Count.ShouldBe(1);
        result.ShouldContain(content1);
        result.ShouldAllBe(item => item is Sponsor);
    }

    [Fact]
    public void GetElements_FromMultipleMediaWithCrops_ShouldReturnEmpty_WhenNoMatchingTypesExist()
    {
        // Arrange
        var wrongContent = A.Fake<Models.Content.File>();
        var fallback = A.Fake<IPublishedValueFallback>();
        var crops = new ImageCropperValue();

        var media = new MediaWithCrops(wrongContent, fallback, crops);

        // Act
        var result = new[] { media }.GetElements<Sponsor>().ToList();

        // Assert
        result.ShouldBeEmpty();
    }
}
