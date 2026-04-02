using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Jalles.Core.Extensions;
using Jalles.Core.Contracts;
using Jalles.Core.Models.Content;

namespace Jalles.Core.Tests.Extensions;

public class PublishedElementExtensionsTests
{
    [Fact]
    public void GetString_ReturnsPropertyValue_WhenPropertyExistsAndIsString()
    {
        // Arrange
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => element.GetProperty("title")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns("Hello World");

        // Act
        var result = element.GetString("title");

        // Assert
        result.ShouldBe("Hello World");
        result.ShouldBeOfType<string>();
    }

    [Fact]
    public void GetString_ReturnsEmptyString_WhenPropertyDoesNotExist()
    {
        // Arrange
        var element = A.Fake<IPublishedElement>();
        A.CallTo(() => element.GetProperty("missing")).Returns(null);

        // Act
        var result = element.GetString("missing");

        // Assert
        result.ShouldBe(string.Empty);
        result.ShouldBeOfType<string>();
    }

    [Fact]
    public void GetString_ReturnsEmptyString_WhenPropertyValueIsNotString()
    {
        // Arrange
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => element.GetProperty("number")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(123);

        // Act
        var result = element.GetString("number");

        // Assert
        result.ShouldBe(string.Empty);
        result.ShouldBeOfType<string>();
    }

    [Fact]
    public void GetString_ReturnsEmptyString_WhenElementIsNull()
    {
        // Act
        var result = PublishedElementExtensions.GetString(null!, "anything");

        // Assert
        result.ShouldBe(string.Empty);
        result.ShouldBeOfType<string>();
    }

    [Fact]
    public void GetString_ReturnsValueFromParent_WhenCurrentContentHasNoValue()
    {
        // Arrange
        var contentAccessor = A.Fake<IContentAccessor>();

        var parent = A.Fake<IPublishedContent>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => parent.GetProperty("title")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns("Parent Value");

        var current = A.Fake<IPublishedContent>();
        var currentProperty = A.Fake<IPublishedProperty>();
        A.CallTo(() => current.GetProperty("title")).Returns(currentProperty);
        A.CallTo(() => currentProperty.GetValue(null, null)).Returns(""); // No value on current

        A.CallTo(() => contentAccessor.GetParent<IPublishedContent>(current)).Returns(parent);
        A.CallTo(() => contentAccessor.GetParent<IPublishedContent>(parent)).Returns(null);

        // Act
        var result = current.GetString("title", contentAccessor);

        // Assert
        result.ShouldBe("Parent Value");
        result.ShouldBeOfType<string>();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetBool_ReturnsBoolValue_WhenPropertyIsBool(bool value)
    {
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => element.GetProperty("isActive")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(value);

        var result = element.GetBool("isActive");
        result.ShouldBe(value);
        result.ShouldBeOfType<bool>();
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("TRUE", true)]
    [InlineData("FALSE", false)]
    public void GetBool_ReturnsBoolValue_WhenPropertyIsStringBool(string value, bool expected)
    {
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => element.GetProperty("isActive")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(value);

        var result = element.GetBool("isActive");
        result.ShouldBe(expected);
        result.ShouldBeOfType<bool>();
    }

    [Fact]
    public void GetBool_ReturnsFalse_WhenPropertyIsNull()
    {
        var element = A.Fake<IPublishedElement>();
        A.CallTo(() => element.GetProperty("isActive")).Returns(null);

        var result = element.GetBool("isActive");
        result.ShouldBeFalse();
        result.ShouldBeOfType<bool>();
    }

    [Fact]
    public void GetBool_ReturnsFalse_WhenPropertyIsNonBoolNonString()
    {
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => element.GetProperty("isActive")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(123);

        var result = element.GetBool("isActive");
        result.ShouldBeFalse();
        result.ShouldBeOfType<bool>();
    }

    [Fact]
    public void GetMediaWithCrops_ReturnsMediaWithCrops_WhenPropertyIsMediaWithCrops()
    {
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        var content = A.Fake<IPublishedContent>();
        var fallback = A.Fake<IPublishedValueFallback>();
        var crops = new ImageCropperValue { Src = "/media/abc.jpg" };
        var mediaWithCrops = new MediaWithCrops(content, fallback, crops);

        A.CallTo(() => element.GetProperty("media")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(mediaWithCrops);

        var result = element.GetMediaWithCrops("media");
        result.ShouldBe(mediaWithCrops);
        result.ShouldBeOfType<MediaWithCrops>();
    }

    [Fact]
    public void GetMediaWithCrops_ReturnsNull_WhenPropertyIsNull()
    {
        var element = A.Fake<IPublishedElement>();
        A.CallTo(() => element.GetProperty("media")).Returns(null);

        var result = element.GetMediaWithCrops("media");
        result.ShouldBeNull();
    }

    [Fact]
    public void GetMediaWithCrops_ReturnsNull_WhenPropertyValueIsNotMediaWithCrops()
    {
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        A.CallTo(() => element.GetProperty("media")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns("not media");

        var result = element.GetMediaWithCrops("media");
        result.ShouldBeNull();
    }

    [Fact]
    public void GetBlockListItem_ReturnsBlockListItemOfCorrectType_WhenBlockListItemHasMatchingType()
    {
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        var headerBlock = A.Fake<HeaderBlock>();
        var blockListItem = new BlockListItem<HeaderBlock>(Guid.NewGuid(), headerBlock, null, null);

        A.CallTo(() => element.GetProperty("header")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(blockListItem);

        var result = element.GetBlockListItem<HeaderBlock>("header");
        result.ShouldBe(blockListItem);
        result.ShouldBeOfType<BlockListItem<HeaderBlock>>();
    }

    [Fact]
    public void GetBlockListItem_ReturnsNull_WhenPropertyIsNull()
    {
        var element = A.Fake<IPublishedElement>();
        A.CallTo(() => element.GetProperty("header")).Returns(null);

        var result = element.GetBlockListItem<HeaderBlock>("header");
        result.ShouldBeNull();
    }

    [Fact]
    public void GetBlockListItem_ReturnsNull_WhenBlockListItemIsNotMatchingType()
    {
        var element = A.Fake<IPublishedElement>();
        var property = A.Fake<IPublishedProperty>();
        var notHeaderBlock = A.Fake<FooterBlock>();
        var blockListItem = new BlockListItem(Guid.NewGuid(), notHeaderBlock, null, null);

        A.CallTo(() => element.GetProperty("header")).Returns(property);
        A.CallTo(() => property.GetValue(null, null)).Returns(blockListItem);

        var result = element.GetBlockListItem<HeaderBlock>("header");
        result.ShouldBeNull();
    }
}
