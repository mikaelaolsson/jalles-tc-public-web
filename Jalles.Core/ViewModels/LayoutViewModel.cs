using Jalles.Core.Extensions;
using Jalles.Core.Models.Content;
using Jalles.Core.ViewModels.Blocks;
using Polly;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
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

        var headerProperty = content?.Value<BlockListItem<HeaderBlock>>("header")?.Content;
        Header = headerProperty.GetMediaForHeader();
    }

    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public string Url { get; set; }
    public string Thumbnail { get; set; }
    public MediaViewModel Header { get; set; }
}

public class LayoutViewModel<T> : LayoutViewModel where T : class
{
    protected LayoutViewModel(T viewModel, IPublishedContent content, HttpContext context) : base(content, context)
    {
        ViewModel = viewModel;
    }

    public T ViewModel { get; set; }

    public static Task<LayoutViewModel<T>> CreateAsync(T viewModel, IPublishedContent content, HttpContext context)
    {
        var model = new LayoutViewModel<T>(viewModel, content, context);

        return Task.FromResult(model);
    }
}