using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Web;
using Umbraco.Community.Contentment.DataEditors;

namespace Jalles.Web.DataSource;

public class PropertyDataDataSource : IDataListSource
{
    private readonly IUmbracoContextFactory _umbracoContextFactory;

    public PropertyDataDataSource(IUmbracoContextFactory umbracoContextFactory)
    {
        _umbracoContextFactory = umbracoContextFactory;
    }
    public string Name => "Custom Filter Data";
    public string Description => "Data source for umbraco property data coming from the Listing Pages.";
    public string Icon => "icon-filter";
    public Dictionary<string, object> DefaultValues => new();

    public string Group => "Custom Filters";
    public OverlaySize OverlaySize => OverlaySize.Small;
    public IEnumerable<ConfigurationField> Fields => new[]
    {
        new ConfigurationField
        {
            Key = "propAlias",
            Name = "Property alias",
            Description = "The property alias",
            View = "textstring"
        }
    };

    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config)
    {
        using var umbracoContextReference = _umbracoContextFactory.EnsureUmbracoContext();
        var guid = Guid.Parse("c764a2ff-a9b8-4491-b84c-d89e30fc17b1"); // dev aktuellt
        var content = umbracoContextReference.UmbracoContext.Content?.GetById(guid);

        var propertyAlias = config["propAlias"].ToString();
        if (string.IsNullOrWhiteSpace(propertyAlias))
            throw new ArgumentException("Property alias must not be null or empty", nameof(propertyAlias));

        var prop = content?.GetProperty(propertyAlias!);
        return prop == null
            ? throw new NullReferenceException("Property not found on node")
            : (content?.GetProperty(propertyAlias!)?.GetValue() as string[] ?? Array.Empty<string>())
            .Select(value => new DataListItem { Name = value, Value = value }).ToList();
    }
}
