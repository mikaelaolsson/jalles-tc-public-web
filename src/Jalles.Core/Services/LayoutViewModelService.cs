using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Jalles.Core.Contracts;
using Jalles.Core.Extensions;
using Jalles.Core.Constants;

namespace Jalles.Core.Services;

public class LayoutViewModelService : ILayoutViewModelService
{
    private readonly IUmbracoPagePathService _umbracoPagePathService;
    private readonly IContentAccessor _contentAccessor;
    private readonly IMapper _mapper;

    public LayoutViewModelService(
        IUmbracoPagePathService umbracoPagePathService,
        IContentAccessor contentAccessor,
        IMapper mapper)
    {
        _umbracoPagePathService = umbracoPagePathService;
        _contentAccessor = contentAccessor;
        _mapper = mapper;
    }

    public string GetTitle(IPublishedContent? content)
    {
        var titleProperty = content?.GetString("title");
        var headingProperty = content?.GetString("heading");

        return !string.IsNullOrWhiteSpace(titleProperty)
            ? titleProperty
            : !string.IsNullOrWhiteSpace(headingProperty)
                ? headingProperty
                : content?.Name ?? string.Empty;
    }

    public string GetMetaDescription(IPublishedContent? content)
        => content?.GetString("metaDescription", _contentAccessor) ?? string.Empty;

    public string GetUrl(IPublishedContent? content, HttpContext context)
    {
        return content != null
            ? _umbracoPagePathService.GetPageUri(content).ToString()
            : GetBaseUrl(context);
    }

    public string GetThumbnail(IPublishedContent? content, HttpContext context)
    {
        var baseUrl = GetBaseUrl(context);
        var thumbnail = content?.GetMediaWithCrops("thumbnail");
        var thumbCropUrl = _umbracoPagePathService.GetCropUrl(thumbnail, "thumbnail");

        if(string.IsNullOrWhiteSpace(thumbCropUrl))
            return baseUrl + JallesConstants.DefaultFallbackThumbnail;

        return Uri.IsWellFormedUriString(thumbCropUrl, UriKind.Absolute)
            ? thumbCropUrl
            : baseUrl + thumbCropUrl;
    }

    public HeaderViewModel BuildHeader(IPublishedContent? content, CultureInfo culture)
    {
        var headerProperty = content?.GetBlockListItem<HeaderBlock>("header")?.Content;

        return new HeaderViewModel
        {
            MediaBlock = GetMediaForHeader(headerProperty, culture),
            Content = content
        };
    }

    private static string GetBaseUrl(HttpContext context) => "https://" + context.Request.Host;

    private MediaBlockViewModel GetMediaForHeader(HeaderBlock? header, CultureInfo culture)
    {
        if(header == null)
            return new MediaBlockViewModel();

        var mediaType = header.GetMediaType();
        var mediaSource = _umbracoPagePathService.GetMediaUrl(header.Media, culture);

        if(string.IsNullOrEmpty(mediaSource))
        {
            mediaSource = JallesConstants.DefaultFallbackMedia;
        }

        return new MediaBlockViewModel
        {
            Media = header.Media,
            BackgroundColor = header.BackgroundColor.GetMediaBackgroundColor(),
            AddBlurOverlay = header.AddBlurOverlay,
            MediaType = mediaType,
            MediaSource = mediaSource,
            IsLazy = false
        };
    }
}
