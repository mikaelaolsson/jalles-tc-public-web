using System.Globalization;

namespace Jalles.Core.Contracts;

public interface IUmbracoPagePathService
{
    string GetParentPagePath(IPublishedContent? child);
    string GetPagePath(IPublishedContent? content);
    Uri GetPageUri(IPublishedContent content);
    string GetMediaUrl(IPublishedContent? media, CultureInfo culture);
    string GetCropUrl(MediaWithCrops? media, string cropAlias);
}
