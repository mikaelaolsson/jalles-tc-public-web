using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.Tests.Extensions;

public class MediaBlockExtensionsTests
{
    // ISimpleMediaProperties
    [Theory]
    [InlineData("umbracoMediaVideo", MediaType.Video)]
    [InlineData("Image", MediaType.Image)]
    [InlineData("unknownType", MediaType.Image)] // Unknown type
    [InlineData("", MediaType.Image)] // Empty media type
    public void GetMediaType_ISimpleMediaProperties_ShouldReturnCorrectMediaType(string alias, MediaType expected)
    {
        // Arrange
        var media = A.Fake<MediaWithCrops>();
        A.CallTo(() => media.ContentType.Alias).Returns(alias);

        var mediaProps = A.Fake<ISimpleMediaProperties>();
        A.CallTo(() => mediaProps.Media).Returns(media);

        // Act
        var result = mediaProps.GetMediaType();

        // Assert
        result.ShouldBe(expected);
    }

    // IMediaProperties
    [Theory]
    [InlineData("umbracoMediaVideo", MediaType.Video)]
    [InlineData("Image", MediaType.Image)]
    [InlineData("unknownType", MediaType.Image)] // Unknown type
    [InlineData("", MediaType.Image)] // Empty media type
    public void GetMediaType_IMediaProperties_ShouldReturnCorrectMediaType(string alias, MediaType expected)
    {
        // Arrange
        var media = A.Fake<MediaWithCrops>();
        A.CallTo(() => media.ContentType.Alias).Returns(alias);

        var mediaProps = A.Fake<IMediaProperties>();
        A.CallTo(() => mediaProps.Media).Returns(media);

        // Act
        var result = mediaProps.GetMediaType();

        // Assert
        result.ShouldBe(expected);
    }

    [Fact]
    public void GetMediaType_ShouldReturnBackgroundColor_WhenMediaIsNullButBackgroundColorExists()
    {
        // Arrange
        var mediaProps = A.Fake<IMediaProperties>();
        A.CallTo(() => mediaProps.Media).Returns(null);
        A.CallTo(() => mediaProps.BackgroundColor).Returns(new ColorPickerValueConverter.PickedColor("#ffcc00", "Pikachu yellow!"));

        // Act
        var result = mediaProps.GetMediaType();

        // Assert
        result.ShouldBe(MediaType.BackgroundColor);
    }

    [Theory]
    [InlineData("https://vimeo.com/123456789", "https://player.vimeo.com/video/123456789?title=0&byline=0&portrait=0")]
    [InlineData("https://vimeo.com/showcase/987654321", "https://vimeo.com/showcase/987654321/embed")]
    [InlineData("https://youtu.be/ABCdef123", "https://www.youtube.com/embed/ABCdef123")]
    [InlineData("https://www.youtube.com/watch?v=XYZ789", "https://www.youtube.com/embed/XYZ789")]
    [InlineData("https://notavideo.com/watch?v=fake", "")] // Invalid video source
    [InlineData(null, "")] // No video provided
    public void GetMediaSource_ForVideo_ShouldReturnCorrectEmbedUrl(string videoLink, string expected)
    {
        // Arrange
        var videoProps = A.Fake<IVideoBlockProperties>();
        A.CallTo(() => videoProps.VideoUrl).Returns(videoLink);

        // Act
        var result = videoProps.GetMediaSource();

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("#ffcc00", "#ffcc00")]
    [InlineData("ffcc00", "#ffcc00")]
    [InlineData("#FFF", "#FFF")]
    [InlineData("", "")]
    [InlineData(null, "")]
    [InlineData("notAColor", "")]
    public void GetMediaBackgroundColor_ReturnsExpected(string input, string expected)
    {
        // Arrange
        var pickedColor = input is null ? null : new ColorPickerValueConverter.PickedColor(input, "Test");

        // Act
        var result = pickedColor.GetMediaBackgroundColor();

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData("FFEB19", "color-jalles-yellow")]
    [InlineData("FAFAEC", "color-off-white")]
    [InlineData("FFF8AD", "color-vanilla")]
    [InlineData("463F3A", "color-taupe")]
    [InlineData("8A817C", "color-battleship-gray")]
    [InlineData("1E2022", "color-black")]
    [InlineData("ABCDEF", "color-off-white")]
    [InlineData(null, "color-off-white")]
    public void GetBackgroundColorName_ReturnsExpected(string input, string expected)
    {
        // Arrange
        var pickedColor = input is null ? null : new ColorPickerValueConverter.PickedColor(input, "Test");

        // Act
        var result = pickedColor.GetBackgroundColorName();

        // Assert
        result.ShouldBe(expected);
    }
}
