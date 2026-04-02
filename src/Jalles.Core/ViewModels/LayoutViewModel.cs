using System.Globalization;
using Microsoft.AspNetCore.Http;
using Jalles.Core.Contracts;

namespace Jalles.Core.ViewModels;

public class LayoutViewModel : ContentModel
{
    public LayoutViewModel(
        IPublishedContent content,
        HttpContext context,
        ILayoutViewModelService layoutViewModelService)
        : base(content)
    {
        var culture = CultureInfo.CurrentUICulture;

        Title = layoutViewModelService.GetTitle(content);
        MetaDescription = layoutViewModelService.GetMetaDescription(content);
        Url = layoutViewModelService.GetUrl(content, context);
        Thumbnail = layoutViewModelService.GetThumbnail(content, context);
        Header = layoutViewModelService.BuildHeader(content, culture);
    }

    public string Title { get; set; }
    public string MetaDescription { get; set; }
    public string Url { get; set; }
    public string Thumbnail { get; set; }
    public HeaderViewModel Header { get; set; }
}

public class LayoutViewModel<T> : LayoutViewModel where T : class
{
    protected LayoutViewModel(T viewModel, IPublishedContent content, HttpContext context, ILayoutViewModelService layoutViewModelService)
        : base(content, context, layoutViewModelService)
    {
        ViewModel = viewModel;
    }

    public T ViewModel { get; set; }

    public static Task<LayoutViewModel<T>> CreateAsync(T viewModel, IPublishedContent content, HttpContext context, ILayoutViewModelService layoutViewModelService)
    {
        var model = new LayoutViewModel<T>(viewModel, content, context, layoutViewModelService);
        return Task.FromResult(model);
    }
}
