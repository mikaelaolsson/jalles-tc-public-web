using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace Jalles.Core.Contracts;

public interface ILayoutViewModelService
{
    string GetTitle(IPublishedContent? content);
    string GetMetaDescription(IPublishedContent? content);
    string GetUrl(IPublishedContent? content, HttpContext context);
    string GetThumbnail(IPublishedContent? content, HttpContext context);
    HeaderViewModel BuildHeader(IPublishedContent? content, CultureInfo culture);
}
