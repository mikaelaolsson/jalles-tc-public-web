using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using System.Text.RegularExpressions;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.Extensions;

public static class MediaBlockExtensions
{
    private const string _defaultFallbackMedia = "/images/jalles-media.jpg";

    public static MediaViewModel GetMediaForHeader(this HeaderBlock? headerBlock)
    {
        if (headerBlock == null) return new MediaViewModel();

        var mediaType = headerBlock.GetMediaType();
        return new MediaViewModel
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
}
