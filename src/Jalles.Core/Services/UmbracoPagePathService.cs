using System.Globalization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services.Navigation;
using Jalles.Core.Contracts;

namespace Jalles.Core.Services;

public class UmbracoPagePathService : IUmbracoPagePathService
{
    private readonly IPublishedUrlProvider _publishedUrlProvider;
    private readonly IImageUrlGenerator _imageUrlGenerator;
    private readonly IDocumentNavigationQueryService _documentNavigationQueryService;
    private readonly IPublishedContentQuery _publishedContentQuery;

    public UmbracoPagePathService(
        IPublishedUrlProvider publishedUrlProvider,
        IImageUrlGenerator imageUrlGenerator,
        IDocumentNavigationQueryService documentNavigationQueryService,
        IPublishedContentQuery publishedContentQuery)
    {
        _publishedUrlProvider = publishedUrlProvider;
        _imageUrlGenerator = imageUrlGenerator;
        _documentNavigationQueryService = documentNavigationQueryService;
        _publishedContentQuery = publishedContentQuery;
    }

    public string GetParentPagePath(IPublishedContent? content)
    {
        if(content == null)
            return "/";

        return !_documentNavigationQueryService.TryGetAncestorsKeys(content.Key, out var keys) ?
            "/" :
            BuildPathFromKeys(keys);
    }

    public string GetPagePath(IPublishedContent? content)
    {
        if(content == null)
            return "/";

        if(!_documentNavigationQueryService.TryGetAncestorsOrSelfKeys(content.Key, out var ancestorOrSelfKeys))
            return "/";

        return BuildPathFromKeys(ancestorOrSelfKeys);
    }

    public Uri GetPageUri(IPublishedContent content)
    {
        var url = _publishedUrlProvider.GetUrl(content, UrlMode.Absolute);
        return new UriBuilder(url).Uri;
    }

    public string GetMediaUrl(IPublishedContent? media, CultureInfo culture)
    {
        if(media == null)
            return string.Empty;

        const string propertyAlias = "umbracoFile";
        const UrlMode urlMode = UrlMode.Default;

        var url = _publishedUrlProvider.GetMediaUrl(media, urlMode, culture.Name, propertyAlias);
        return url ?? string.Empty;
    }

    public string GetCropUrl(MediaWithCrops? media, string cropAlias)
    {
        if(media == null)
            return string.Empty;

        var crop = media.LocalCrops.Crops?.FirstOrDefault(c => c.Alias == cropAlias);

        var options = new ImageUrlGenerationOptions(media.LocalCrops.Src);

        if(crop?.Width > 0 && crop.Height > 0)
        {
            options.Width = crop.Width;
            options.Height = crop.Height;
        }

        var cropUrl = _imageUrlGenerator.GetImageUrl(options);

        return !string.IsNullOrWhiteSpace(cropUrl) ?
            cropUrl :
            !string.IsNullOrWhiteSpace(media.LocalCrops.Src) ?
                media.LocalCrops.Src :
                string.Empty;
    }

    private string BuildPathFromKeys(IEnumerable<Guid> keys)
    {
        var segments = keys
            .Select(key => _publishedContentQuery.Content(key))
            .Where(c => c != null && c.ContentType.Alias != "startPage")
            .Reverse()
            .Select(c => c!.UrlSegment)
            .ToList();

        return segments.Count > 0 ? $"/{string.Join("/", segments)}/" : "/";
    }
}
