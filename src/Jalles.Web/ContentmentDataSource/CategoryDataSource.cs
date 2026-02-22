using Jalles.Core.Contracts;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Community.Contentment.DataEditors;

namespace Jalles.Web.ContentmentDataSource;

public class CategoryDataSource : IContentmentDataSource
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    private HttpContext HttpContext =>
        _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("Requires a valid HttpContext.");

    private IContentAccessor ContentAccessor =>
        HttpContext.RequestServices.GetRequiredService<IContentAccessor>();

    public CategoryDataSource(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Name => "Category";
    public string Description => "Data source for umbraco property data coming from the Listing Pages.";
    public string Icon => "icon-filter color-purple";
    public Dictionary<string, object> DefaultValues => [];

    public string Group => "Custom Filters";
    public OverlaySize OverlaySize => OverlaySize.Small;

    public IEnumerable<ContentmentConfigurationField> Fields =>
    [
        new ContentmentConfigurationField
        {
            Key = "propAlias",
            Name = "Property alias",
            Description = "The Property alias",
            PropertyEditorUiAlias = "Umb.PropertyEditorUi.TextArea"
        }
    ];

    public IEnumerable<DataListItem> GetItems(Dictionary<string, object> config)
    {
        var listingPages = ContentAccessor.GetChildrenOfType<StartPage, ListingPage>();
        var guid = Guid.Parse("c764a2ff-a9b8-4491-b84c-d89e30fc17b1"); // Prod Aktuellt
        var aktuellt = listingPages.FirstOrDefault(x => x.Key == guid);

        if(aktuellt == null)
            return [];

        var categories = aktuellt.Categories;

        return categories?.Select(category => new DataListItem
        {
            Name = category,
            Value = category,
            Icon = "icon-filter color-purple"
        }).ToList() ?? [];
    }
}
