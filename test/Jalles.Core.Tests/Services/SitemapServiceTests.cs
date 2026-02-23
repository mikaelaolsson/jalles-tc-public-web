using System.Xml.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Jalles.Core.Services;
using Jalles.Core.Contracts;
using Jalles.Core.Constants;
using Jalles.Core.Models.Content;

namespace Jalles.Core.Tests.Services;

public class SitemapServiceTests
{
    private const string _baseUrl = $"http://{JallesConstants.PublicDomain}/";
    private readonly SitemapService _sitemapService;
    private readonly IContentAccessor _contentAccessor;
    private readonly IUmbracoPagePathService _umbracoPagePathService;

    public SitemapServiceTests()
    {
        _contentAccessor = A.Fake<IContentAccessor>();
        _umbracoPagePathService = A.Fake<IUmbracoPagePathService>();

        _sitemapService = new SitemapService(_contentAccessor, _umbracoPagePathService);
    }

    private static Uri BuildMockPageUri(IPublishedContent content, Dictionary<IPublishedContent, IPublishedContent> hierarchy)
    {
        var segments = new List<string>();
        var current = content;

        while(current != null && hierarchy.TryGetValue(current, out var parent))
        {
            var alias = current.ContentType?.Alias ?? string.Empty;
            if(!string.Equals(alias, "startPage", StringComparison.OrdinalIgnoreCase))
            {
                segments.Insert(0, current.UrlSegment);
            }
            current = parent;
        }

        var path = segments.Count > 0 ? "/" + string.Join("/", segments) : "/";
        return new Uri(_baseUrl.TrimEnd('/') + path);
    }

    [Fact]
    public void GetEntries_ShouldExclude_WhenPageNamedSitemap()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var sitemap = A.Fake<XmlSiteMap>();

        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [sitemap];
            return [];
        });

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => sitemap.UpdateDate).Returns(new DateTime(2020, 2, 1));

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => sitemap.Name).Returns("Sitemap");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => sitemap.ContentType.Alias).Returns("xmlSitemap");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null }, { sitemap, root } };
        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => sitemap.UrlSegment).Returns("sitemap");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var entries = _sitemapService.GetEntries(root, []).ToList();

        // Assert
        entries.Count.ShouldBe(1);
        entries.Any(e => e.Loc == _baseUrl).ShouldBeTrue();
        entries.Any(e => e.Loc == _baseUrl + "sitemap").ShouldBeFalse();
    }

    [Fact]
    public void GetEntries_ShouldExclude_WhenDocumentTypeInExclusionList()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var errorPage = A.Fake<ErrorPage>();

        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [errorPage];
            return [];
        });

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => errorPage.UpdateDate).Returns(new DateTime(2020, 2, 1));

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => errorPage.Name).Returns("Error Page");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => errorPage.ContentType.Alias).Returns("errorPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null }, { errorPage, root } };
        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => errorPage.UrlSegment).Returns("error-page");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var entries = _sitemapService.GetEntries(root, ["ErrorPage"]).ToList();

        // Assert
        entries.Count.ShouldBe(1);
        entries.Any(e => e.Loc == _baseUrl).ShouldBeTrue();
        entries.Any(e => e.Loc == _baseUrl + "error-page").ShouldBeFalse();
    }

    [Fact]
    public void GetEntries_ShouldIncludeRoot_WhenRootExists()
    {
        // Arrange
        var root = A.Fake<StartPage>();

        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).Returns([]);
        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => root.ContentType.Alias).Returns("startPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null } };
        A.CallTo(() => root.UrlSegment).Returns("hem");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var entries = _sitemapService.GetEntries(root, []).ToList();

        // Assert
        entries.Count.ShouldBe(1);
        entries.First().Loc.ShouldBe(_baseUrl);
    }

    [Fact]
    public void GetEntries_ShouldIncludeVisibleChildren_WhenTheyAreNotExcluded()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var visibleChild = A.Fake<ContentPage>();

        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [visibleChild];
            return [];
        });

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => visibleChild.UpdateDate).Returns(new DateTime(2020, 2, 1));

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => visibleChild.Name).Returns("Visible Child");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => visibleChild.ContentType.Alias).Returns("contentPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null }, { visibleChild, root } };
        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => visibleChild.UrlSegment).Returns("visible-child");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var entries = _sitemapService.GetEntries(root, []).ToList();

        // Assert
        entries.Count.ShouldBe(2);
        entries.Any(e => e.Loc == _baseUrl).ShouldBeTrue();
        entries.Any(e => e.Loc == _baseUrl + "visible-child").ShouldBeTrue();
    }

    [Fact]
    public void GetEntries_ShouldReturnCorrectLastModDate()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var child = A.Fake<ContentPage>();
        var minValueChild = A.Fake<ContentPage>();

        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [child, minValueChild];
            return [];
        });

        var rootUpdateDate = new DateTime(2020, 1, 15);
        var childUpdateDate = new DateTime(2020, 3, 25);
        A.CallTo(() => root.UpdateDate).Returns(rootUpdateDate);
        A.CallTo(() => child.UpdateDate).Returns(childUpdateDate);
        A.CallTo(() => minValueChild.UpdateDate).Returns(DateTime.MinValue);

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => child.Name).Returns("Child");
        A.CallTo(() => minValueChild.Name).Returns("MinValue Child");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => child.ContentType.Alias).Returns("contentPage");
        A.CallTo(() => minValueChild.ContentType.Alias).Returns("contentPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null }, { child, root }, { minValueChild, root } };
        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => child.UrlSegment).Returns("child");
        A.CallTo(() => minValueChild.UrlSegment).Returns("minvalue-child");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var entries = _sitemapService.GetEntries(root, []).ToList();

        // Assert
        var rootEntry = entries.First(e => e.Loc == _baseUrl);
        rootEntry.LastMod.UtcDateTime.Date.ShouldBe(rootUpdateDate);

        var childEntry = entries.First(e => e.Loc == _baseUrl + "child");
        childEntry.LastMod.UtcDateTime.Date.ShouldBe(childUpdateDate);

        var minValueEntry = entries.First(e => e.Loc == _baseUrl + "minvalue-child");
        minValueEntry.LastMod.ShouldBeGreaterThanOrEqualTo(DateTimeOffset.UtcNow.AddSeconds(-5));
    }

    [Fact]
    public void GetEntries_ShouldRecurseThroughMultipleLevels_WhenAllAreVisible()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var level1 = A.Fake<ContentPage>();
        var level2 = A.Fake<ContentPage>();
        var level3 = A.Fake<ContentPage>();

        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [level1];
            if(ReferenceEquals(parent, level1)) return [level2];
            if(ReferenceEquals(parent, level2)) return [level3];
            return [];
        });

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => level1.UpdateDate).Returns(new DateTime(2020, 2, 1));
        A.CallTo(() => level2.UpdateDate).Returns(new DateTime(2020, 3, 1));
        A.CallTo(() => level3.UpdateDate).Returns(new DateTime(2020, 4, 1));

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => level1.Name).Returns("Level 1");
        A.CallTo(() => level2.Name).Returns("Level 2");
        A.CallTo(() => level3.Name).Returns("Level 3");

        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => level1.UrlSegment).Returns("level-1");
        A.CallTo(() => level2.UrlSegment).Returns("level-2");
        A.CallTo(() => level3.UrlSegment).Returns("level-3");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => level1.ContentType.Alias).Returns("contentPage");
        A.CallTo(() => level2.ContentType.Alias).Returns("contentPage");
        A.CallTo(() => level3.ContentType.Alias).Returns("contentPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent>
        {
            { root, null },
            { level1, root },
            { level2, level1 },
            { level3, level2 }
        };

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call =>
            {
                var content = (IPublishedContent)call.Arguments[0];
                return BuildMockPageUri(content, hierarchy);
            });

        // Act
        var entries = _sitemapService.GetEntries(root, []).ToList();

        // Assert
        entries.Count.ShouldBe(4);
        entries.Any(e => e.Loc == _baseUrl).ShouldBeTrue();
        entries.Any(e => e.Loc == _baseUrl + "level-1").ShouldBeTrue();
        entries.Any(e => e.Loc == _baseUrl + "level-1/level-2").ShouldBeTrue();
        entries.Any(e => e.Loc == _baseUrl + "level-1/level-2/level-3").ShouldBeTrue();
    }

    [Fact]
    public void GetEntries_ShouldReturnEmpty_WhenRootIsNull()
    {
        // Act
        var entries = _sitemapService.GetEntries(null!, []).ToList();

        // Assert
        entries.ShouldBeEmpty();
    }

    [Fact]
    public void RenderXml_ShouldReturnEmpty_WhenModelIsNull()
    {
        // Act
        var result = _sitemapService.RenderXml(null!);

        // Assert
        result.ShouldBe(string.Empty);
    }

    [Fact]
    public void RenderXml_ShouldReturnValidXml_WhenModelHasNoExcludedTypes()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var child = A.Fake<ContentPage>();
        var model = A.Fake<XmlSiteMap>();

        A.CallTo(() => model.ExcludedDocumentTypes).Returns(null);
        A.CallTo(() => _contentAccessor.GetRoot()).Returns(root);
        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [child];
            return [];
        });

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => child.UpdateDate).Returns(new DateTime(2020, 2, 1));

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => child.Name).Returns("Child");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => child.ContentType.Alias).Returns("contentPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null }, { child, root } };
        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => child.UrlSegment).Returns("child");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var result = _sitemapService.RenderXml(model);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        var doc = XDocument.Parse(result);

        doc.Root.ShouldNotBeNull();
        doc.Root.Name.LocalName.ShouldBe("urlset");

        var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
        var urls = doc.Descendants(ns + "url").ToList();
        urls.Count.ShouldBe(2);

        var locs = urls.Select(u => u.Element(ns + "loc")?.Value).ToList();
        locs.ShouldContain(_baseUrl);
        locs.ShouldContain(_baseUrl + "child");

        var lastmods = urls.Select(u => u.Element(ns + "lastmod")).ToList();
        lastmods.Count.ShouldBe(2);
        lastmods.All(lm => lm != null).ShouldBeTrue();
    }

    [Fact]
    public void RenderXml_ShouldEscapeLocValues()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var model = A.Fake<XmlSiteMap>();

        A.CallTo(() => model.ExcludedDocumentTypes).Returns(null);
        A.CallTo(() => _contentAccessor.GetRoot()).Returns(root);
        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).Returns([]);

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => root.ContentType.Alias).Returns("startPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null } };
        A.CallTo(() => root.UrlSegment).Returns("hem");

        var unsafeUrl = "http://www.jallestc.se/?param=value&other=test";
        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .Returns(new Uri(unsafeUrl));

        // Act
        var result = _sitemapService.RenderXml(model);

        // Assert
        result.ShouldNotBeNullOrEmpty();

        // Verify that special XML characters are escaped in the raw XML
        result.ShouldContain("&amp;other=test");  // & should be escaped to &amp;
        result.ShouldNotContain("&other=test");  // Raw & should not appear unescaped

        var doc = XDocument.Parse(result);
        doc.Root.ShouldNotBeNull();
        doc.Root.Name.LocalName.ShouldBe("urlset");

        var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
        var urls = doc.Descendants(ns + "url").ToList();
        urls.Count.ShouldBe(1);

        var loc = urls[0].Element(ns + "loc")?.Value;
        loc.ShouldBe(unsafeUrl);
    }

    [Fact]
    public void RenderXml_ShouldParseExcludedDocumentTypes()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var excludedPage = A.Fake<ErrorPage>();
        var model = A.Fake<XmlSiteMap>();

        A.CallTo(() => model.ExcludedDocumentTypes).Returns("errorPage, notFoundPage");
        A.CallTo(() => _contentAccessor.GetRoot()).Returns(root);
        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [excludedPage];
            return [];
        });

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => excludedPage.UpdateDate).Returns(new DateTime(2020, 2, 1));

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => excludedPage.Name).Returns("Error");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => excludedPage.ContentType.Alias).Returns("errorPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null }, { excludedPage, root } };
        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => excludedPage.UrlSegment).Returns("error");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var result = _sitemapService.RenderXml(model);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        var doc = XDocument.Parse(result);

        doc.Root.ShouldNotBeNull();
        doc.Root.Name.LocalName.ShouldBe("urlset");

        var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
        var urls = doc.Descendants(ns + "url").ToList();
        urls.Count.ShouldBe(1);

        var locs = urls.ConvertAll(u => u.Element(ns + "loc")?.Value);
        locs.ShouldContain(_baseUrl);
        locs.ShouldNotContain("error");
    }

    [Fact]
    public void RenderXml_ShouldFormatLastModCorrectly()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var model = A.Fake<XmlSiteMap>();

        A.CallTo(() => model.ExcludedDocumentTypes).Returns(null);
        A.CallTo(() => _contentAccessor.GetRoot()).Returns(root);
        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).Returns([]);

        var updateDate = new DateTime(2020, 6, 15, 10, 30, 45);
        A.CallTo(() => root.UpdateDate).Returns(updateDate);
        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => root.ContentType.Alias).Returns("startPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null } };
        A.CallTo(() => root.UrlSegment).Returns("hem");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var result = _sitemapService.RenderXml(model);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        var doc = XDocument.Parse(result);

        doc.Root.ShouldNotBeNull();
        doc.Root.Name.LocalName.ShouldBe("urlset");

        var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
        var urls = doc.Descendants(ns + "url").ToList();
        urls.Count.ShouldBe(1);

        var lastmod = urls[0].Element(ns + "lastmod")?.Value;
        lastmod.ShouldStartWith("2020-06-15T10:30:45");
    }

    [Fact]
    public void RenderXml_ShouldContainProperXmlNamespaces()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var model = A.Fake<XmlSiteMap>();

        A.CallTo(() => model.ExcludedDocumentTypes).Returns(null);
        A.CallTo(() => _contentAccessor.GetRoot()).Returns(root);
        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).Returns([]);

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => root.ContentType.Alias).Returns("startPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent> { { root, null } };
        A.CallTo(() => root.UrlSegment).Returns("hem");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var result = _sitemapService.RenderXml(model);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        var doc = XDocument.Parse(result);

        doc.Root.ShouldNotBeNull();
        doc.Root.Name.LocalName.ShouldBe("urlset");
        doc.Root.Attribute("xmlns")?.Value.ShouldBe("http://www.sitemaps.org/schemas/sitemap/0.9");
        doc.Root.Attribute(XName.Get("xhtml", "http://www.w3.org/2000/xmlns/"))?.Value.ShouldBe("http://www.w3.org/1999/xhtml");
    }

    [Fact]
    public void RenderXml_ShouldIncludeMultipleEntries()
    {
        // Arrange
        var root = A.Fake<StartPage>();
        var child1 = A.Fake<ContentPage>();
        var child2 = A.Fake<ContentPage>();
        var model = A.Fake<XmlSiteMap>();

        A.CallTo(() => model.ExcludedDocumentTypes).Returns(null);
        A.CallTo(() => _contentAccessor.GetRoot()).Returns(root);
        A.CallTo(() => _contentAccessor.GetAllChildren(A<IPublishedContent>._)).ReturnsLazily(call =>
        {
            var parent = call.Arguments[0] as IPublishedContent;
            if(ReferenceEquals(parent, root)) return [child1, child2];
            return [];
        });

        A.CallTo(() => root.UpdateDate).Returns(new DateTime(2020, 1, 1));
        A.CallTo(() => child1.UpdateDate).Returns(new DateTime(2020, 2, 1));
        A.CallTo(() => child2.UpdateDate).Returns(new DateTime(2020, 3, 1));

        A.CallTo(() => root.Name).Returns("Home");
        A.CallTo(() => child1.Name).Returns("Child 1");
        A.CallTo(() => child2.Name).Returns("Child 2");

        A.CallTo(() => root.ContentType.Alias).Returns("startPage");
        A.CallTo(() => child1.ContentType.Alias).Returns("contentPage");
        A.CallTo(() => child2.ContentType.Alias).Returns("contentPage");

        var hierarchy = new Dictionary<IPublishedContent, IPublishedContent>
        {
            { root, null },
            { child1, root },
            { child2, root }
        };
        A.CallTo(() => root.UrlSegment).Returns("hem");
        A.CallTo(() => child1.UrlSegment).Returns("child-1");
        A.CallTo(() => child2.UrlSegment).Returns("child-2");

        A.CallTo(() => _umbracoPagePathService.GetPageUri(A<IPublishedContent>._))
            .ReturnsLazily(call => BuildMockPageUri((IPublishedContent)call.Arguments[0], hierarchy));

        // Act
        var result = _sitemapService.RenderXml(model);

        // Assert
        result.ShouldNotBeNullOrEmpty();
        var doc = XDocument.Parse(result);

        doc.Root.ShouldNotBeNull();
        doc.Root.Name.LocalName.ShouldBe("urlset");

        var ns = XNamespace.Get("http://www.sitemaps.org/schemas/sitemap/0.9");
        var urls = doc.Descendants(ns + "url").ToList();
        urls.Count.ShouldBe(3);

        var locs = urls.ConvertAll(u => u.Element(ns + "loc")?.Value);
        locs.ShouldContain(_baseUrl);
        locs.ShouldContain(_baseUrl + "child-1");
        locs.ShouldContain(_baseUrl + "child-2");

        var lastmods = urls.ConvertAll(u => u.Element(ns + "lastmod"));
        lastmods.Count.ShouldBe(3);
        lastmods.All(lm => lm != null).ShouldBeTrue();
    }
}
