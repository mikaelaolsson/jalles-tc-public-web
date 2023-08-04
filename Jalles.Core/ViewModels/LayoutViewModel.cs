using Polly;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Jalles.Core.ViewModels;

public class LayoutViewModel : ContentModel
{
    public LayoutViewModel(IPublishedContent content, HttpContext context) : base(content)
    {
        var titleProperty = content.Value<string>("title");
        Title = !string.IsNullOrWhiteSpace(titleProperty) ? titleProperty : content.Name;
        var metaDescriptionProperty = content.Value<string>("metaDescription", fallback: Fallback.ToAncestors);
        MetaDescription = !string.IsNullOrWhiteSpace(metaDescriptionProperty) ? metaDescriptionProperty : string.Empty;
        var host = "https://" + context.Request.Host;
        Url = host + content.Url();
        Thumbnail = (content?.Value<MediaWithCrops>("thumbnail")?.GetCropUrl("thumbnail") ?? host + "/images/jalles-logo-yellow.png");
    }

    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public string Url { get; set; }
    public string Thumbnail { get; set; }
    //public MediaViewModel Header { get; set; }
}
