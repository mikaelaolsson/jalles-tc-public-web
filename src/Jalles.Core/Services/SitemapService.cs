using System.Text;
using Jalles.Core.Contracts;
using Jalles.Core.Models.Domain;

namespace Jalles.Core.Services;

public class SitemapService : ISitemapService
{
    private readonly IContentAccessor _contentAccessor;
    private readonly IUmbracoPagePathService _umbracoPagePathService;

    public SitemapService(IContentAccessor contentAccessor, IUmbracoPagePathService umbracoPagePathService)
    {
        _contentAccessor = contentAccessor;
        _umbracoPagePathService = umbracoPagePathService;
    }

    public string RenderXml(XmlSiteMap model)
    {
        if(model == null)
            return string.Empty;

        var excludedDocumentTypeList = model.ExcludedDocumentTypes;
        var excludedDocumentTypes = ((!string.IsNullOrEmpty(excludedDocumentTypeList)) ?
            excludedDocumentTypeList.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray() :
            []) ?? [];

        var root = _contentAccessor.GetRoot();
        var entries = GetEntries(root, excludedDocumentTypes);

        var sb = new StringBuilder();
        sb.AppendLine("<urlset xmlns:xhtml=\"http://www.w3.org/1999/xhtml\" xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">\n");
        foreach(var e in entries)
        {
            sb.AppendLine("  <url>");
            sb.AppendLine($"    <loc>{System.Security.SecurityElement.Escape(e.Loc)}</loc>");
            sb.AppendLine($"    <lastmod>{e.LastMod.ToString("yyyy-MM-ddTHH:mm:sszzz")}</lastmod>");
            sb.AppendLine("  </url>");
        }
        sb.AppendLine("</urlset>");
        return sb.ToString();
    }

    internal IEnumerable<SitemapEntry> GetEntries(IPublishedContent root, IEnumerable<string> excludedDocumentTypes)
    {
        if(root == null)
            yield break;

        yield return ToEntry(root);

        foreach(var entry in EnumerateChildren(root, excludedDocumentTypes))
            yield return entry;
    }

    private IEnumerable<SitemapEntry> EnumerateChildren(IPublishedContent parent, IEnumerable<string>? excludedDocumentTypes)
    {
        var children = _contentAccessor.GetAllChildren(parent);
        if(children?.Any() != true)
            yield break;

        foreach(var child in children)
        {
            if(child == null)
                continue;

            if(string.Equals(child.Name, "Sitemap", StringComparison.OrdinalIgnoreCase))
                continue;

            var alias = child.ContentType?.Alias ?? string.Empty;
            var safeExcluded = excludedDocumentTypes ?? [];

            if(safeExcluded.Any(x => string.Equals(x, alias, StringComparison.OrdinalIgnoreCase)))
                continue;

            yield return ToEntry(child);

            var childChildren = _contentAccessor.GetAllChildren(child);
            if(childChildren?.Any() == true)
            {
                foreach(var sub in EnumerateChildren(child, excludedDocumentTypes))
                    yield return sub;
            }
        }
    }

    private SitemapEntry ToEntry(IPublishedContent content)
    {
        var url = _umbracoPagePathService.GetPageUri(content).AbsoluteUri;
        var lastmod = content.UpdateDate == DateTime.MinValue
            ? DateTimeOffset.UtcNow
            : new DateTimeOffset(DateTime.SpecifyKind(content.UpdateDate, DateTimeKind.Utc));

        return new SitemapEntry
        {
            Loc = url,
            LastMod = lastmod
        };
    }
}
