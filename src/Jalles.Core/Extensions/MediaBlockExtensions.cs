using System.Text.RegularExpressions;
using static Umbraco.Cms.Core.PropertyEditors.ValueConverters.ColorPickerValueConverter;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.Extensions;

public static partial class MediaBlockExtensions
{
    public static MediaType GetMediaType(this IMediaProperties? mediaProperties)
    {
        var mediaType = MediaType.Image;
        if(mediaProperties?.Media != null)
        {
            return mediaProperties.Media.ContentType.Alias switch
            {
                "umbracoMediaVideo" => MediaType.Video,
                "Image" => MediaType.Image,
                _ => mediaType
            };
        }

        return !string.IsNullOrEmpty(mediaProperties?.BackgroundColor?.Color) ? MediaType.BackgroundColor : mediaType;
    }

    public static MediaType GetMediaType(this ISimpleMediaProperties? mediaProperties)
    {
        var mediaType = MediaType.Image;
        if(mediaProperties?.Media != null)
        {
            mediaType = mediaProperties.Media.ContentType.Alias switch
            {
                "umbracoMediaVideo" => MediaType.Video,
                "Image" => MediaType.Image,
                _ => mediaType
            };
        }

        return mediaType;
    }

    public static string GetMediaSource(this IVideoBlockProperties? videoProperties)
    {
        if(videoProperties?.VideoUrl is null)
            return string.Empty;

        var videoUrl = videoProperties.VideoUrl;

        var vimeoMatch = VimeoRegex().Match(videoUrl);
        if(vimeoMatch.Success)
        {
            var vimeoVideoId = vimeoMatch.Groups["VideoId"].Value;
            var vimeoShowcaseId = vimeoMatch.Groups["ShowcaseId"].Value;

            if(!string.IsNullOrEmpty(vimeoVideoId))
                return $"https://player.vimeo.com/video/{vimeoVideoId}?title=0&byline=0&portrait=0";
            else if(!string.IsNullOrEmpty(vimeoShowcaseId))
                return $"https://vimeo.com/showcase/{vimeoShowcaseId}/embed";
        }

        var youTubeMatch = YouTubeRegex().Match(videoUrl);
        if(!youTubeMatch.Success)
            return string.Empty;

        var youTubeVideoId = youTubeMatch.Groups["VideoId"].Value;
        return $"https://www.youtube.com/embed/{youTubeVideoId}";
    }

    public static string GetMediaBackgroundColor(this PickedColor? backgroundColor)
    {
        if(string.IsNullOrEmpty(backgroundColor?.Color))
        {
            return string.Empty;
        }

        var color = backgroundColor.Color;

        if(!color.StartsWith("#"))
        {
            color = $"#{color}";
        }

        if(!ColorFormatIsValid(color))
        {
            return string.Empty;
        }

        return color;
    }

    public static string GetBackgroundColorName(this PickedColor? backgroundColor)
    {
        return backgroundColor?.Color?.TrimStart('#').ToUpper() switch
        {
            "FFEB19" => "color-jalles-yellow",
            "3A3418" => "color-pine",
            "7A7F50" => "color-mossy-rock",
            "A79B6E" => "color-granola",
            "F4F2E2" => "color-oat-milk",
            "0A0A0A" => "color-black",
            _ => "color-oat-milk"
        };
    }

    private static bool ColorFormatIsValid(string inputColor)
    {
        return HexColorCodeRegex().IsMatch(inputColor);
    }

    [GeneratedRegex("(?:vimeo\\.com/(?:.*#|.*videos?/|.*channels/.*/)?(?<VideoId>[0-9]+))|(?:vimeo\\.com/showcase/(?<ShowcaseId>[0-9]+))", RegexOptions.CultureInvariant | RegexOptions.Compiled)]
    private static partial Regex VimeoRegex();

    [GeneratedRegex("youtu(?:\\.be|be\\.com)/(?:.*v(?:/|=)|(?:.*/)?)(?<VideoId>[a-zA-Z0-9-_]+)", RegexOptions.CultureInvariant | RegexOptions.Compiled)]
    private static partial Regex YouTubeRegex();

    [GeneratedRegex("^#(?:[0-9a-fA-F]{3}){1,2}$")]
    private static partial Regex HexColorCodeRegex();
}
