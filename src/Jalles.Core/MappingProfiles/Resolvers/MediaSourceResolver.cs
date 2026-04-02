using System.Globalization;
using AutoMapper;
using Jalles.Core.Constants;
using Jalles.Core.Contracts;
using Jalles.Core.Extensions;
using MediaType = Jalles.Core.Enum.MediaType;

namespace Jalles.Core.MappingProfiles.Resolvers;

public class MediaSourceResolver<TSource> : IValueResolver<TSource, MediaBlockViewModel, string>
    where TSource : IPublishedElement
{
    private readonly IUmbracoPagePathService _umbracoPagePathService;

    public MediaSourceResolver(IUmbracoPagePathService umbracoPagePathService)
    {
        _umbracoPagePathService = umbracoPagePathService;
    }

    public string Resolve(TSource source, MediaBlockViewModel destination, string destMember, ResolutionContext context)
    {
        if(source is not IMediaProperties && source is not ISimpleMediaProperties)
        {
            return JallesConstants.DefaultFallbackMedia;
        }

        if(source is IMediaProperties mediaProperties && mediaProperties.GetMediaType() == MediaType.BackgroundColor)
        {
            return mediaProperties.BackgroundColor.GetMediaBackgroundColor();
        }

        MediaType? mediaType = source switch
        {
            IMediaProperties mp => mp.GetMediaType(),
            ISimpleMediaProperties smp => smp.GetMediaType(),
            _ => null
        };

        if(mediaType == null)
        {
            return JallesConstants.DefaultFallbackMedia;
        }

        var culture = CultureInfo.CurrentUICulture;
        var media = source.GetMediaWithCrops("media");

        var mediaSource = _umbracoPagePathService.GetMediaUrl(media, culture);

        return string.IsNullOrWhiteSpace(mediaSource)
            ? JallesConstants.DefaultFallbackMedia
            : mediaSource;
    }
}
