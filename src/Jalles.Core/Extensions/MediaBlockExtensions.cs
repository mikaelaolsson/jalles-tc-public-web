using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using System.Text.RegularExpressions;
using Umbraco.Extensions;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.Extensions;

public static class MediaBlockExtensions
{
    private static readonly Regex _vimeoRegex = new(@"(?:vimeo\.com/(?:.*#|.*videos?/|.*channels/.*/)?(?<VideoId>[0-9]+))|(?:vimeo\.com/showcase/(?<ShowcaseId>[0-9]+))",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex _youTubeRegex = new("youtu(?:\\.be|be\\.com)/(?:.*v(?:/|=)|(?:.*/)?)(?<VideoId>[a-zA-Z0-9-_]+)", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private const string _defaultFallbackMedia = "/static/images/jalles-media.jpg";

    public static MediaBlockViewModel GetMediaForHeader(this HeaderBlock? headerBlock)
    {
        if (headerBlock == null) return new MediaBlockViewModel();

        var mediaType = headerBlock.GetMediaType();
        return new MediaBlockViewModel
        {
            Media = headerBlock.Media,
            BackgroundColor = headerBlock.BackgroundColor.GetMediaBackgroundColor(),
            AddBlurOverlay = headerBlock.AddBlurOverlay,
            MediaType = mediaType,
            MediaSource = headerBlock.GetMediaSource(mediaType),
            IsLazy = false
        };
    }

    public static MediaType GetMediaType(this IMediaProperties? mediaProperties)
    {
        var mediaType = MediaType.Image;
        if (mediaProperties?.Media != null)
        {
            return mediaProperties.Media.ContentType.Alias switch
            {
                "umbracoMediaVideo" => MediaType.Video,
                "Image" => MediaType.Image,
                _ => mediaType
            };
        }

        return !string.IsNullOrEmpty(mediaProperties?.BackgroundColor) ? MediaType.BackgroundColor : mediaType;
    }

    public static MediaType GetMediaType(this ISimpleMediaProperties? mediaProperties)
    {
        var mediaType = MediaType.Image;
        if (mediaProperties?.Media != null)
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

        var vimeoMatch = _vimeoRegex.Match(videoUrl);
        if(vimeoMatch.Success)
        {
            var vimeoVideoId = vimeoMatch.Groups["VideoId"].Value;
            var vimeoShowcaseId = vimeoMatch.Groups["ShowcaseId"].Value;

            if(!string.IsNullOrEmpty(vimeoVideoId))
                return $"https://player.vimeo.com/video/{vimeoVideoId}?title=0&byline=0&portrait=0";
            else if(!string.IsNullOrEmpty(vimeoShowcaseId))
                return $"https://vimeo.com/showcase/{vimeoShowcaseId}/embed";
        }

        var youTubeMatch = _youTubeRegex.Match(videoUrl);
        if(!youTubeMatch.Success)
            return string.Empty;

        var youTubeVideoId = youTubeMatch.Groups["VideoId"].Value;
        return $"https://www.youtube.com/embed/{youTubeVideoId}";
    }

    public static string GetMediaSource(this IMediaProperties mediaProperties, MediaType mediaType)
    {
        var mediaSource = mediaType switch
        {
            MediaType.Video or MediaType.Image => mediaProperties.Media?.MediaUrl(),
            MediaType.BackgroundColor => mediaProperties.BackgroundColor.GetMediaBackgroundColor(),
            _ => ""
        };

        return mediaSource ?? _defaultFallbackMedia;
    }

    public static string GetMediaSource(this ISimpleMediaProperties mediaProperties, MediaType mediaType)
    {
        var mediaSource = mediaType switch
        {
            MediaType.Video or MediaType.Image => mediaProperties.Media?.MediaUrl(),
            _ => ""
        };

        return mediaSource ?? _defaultFallbackMedia;
    }

    public static string GetMediaBackgroundColor(this string? backgroundColor)
    {
        if (string.IsNullOrEmpty(backgroundColor) || !ColorFormatIsValid($"#{backgroundColor}"))
        {
            return string.Empty;
        }

        return $"#{backgroundColor}";
    }

    private static bool ColorFormatIsValid(string inputColor)
    {
        return Regex.Match(inputColor, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success;
    }

    public static string GetBackgroundColorName(this string? backgroundColor)
    {
        return backgroundColor?.ToUpper() switch
        {
            "FFEB19" => "color-jalles-yellow",
            "FAFAEC" => "color-off-white",
            "FFF8AD" => "color-vanilla",
            "463F3A" => "color-taupe",
            "8A817C" => "color-battleship-gray",
            "1E2022" => "color-black",
            _ => "color-off-white"
        };
    }
}
