using AutoMapper;
using Jalles.Core.Contracts;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Jalles.Web.Controllers;
public class ListingPageController : RenderControllerBase
{
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IMapper _mapper;
    private readonly IFilterService _filterService;

    public ListingPageController(ICompositeViewEngine compositeViewEngine, 
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        IFilterService filterService,
        ILogger<RenderController> logger) : base(compositeViewEngine, umbracoContextAccessor, logger)
    {
        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
        _filterService = filterService;
    }

    public async Task<IActionResult> IndexAsync(string category, int page)
    {
        var listingPage = new ListingPage(CurrentPage, _publishedValueFallback);

        var viewModel = _mapper.Map<ListingPageViewModel>(listingPage);

        if (!string.IsNullOrEmpty(category) && category != "Alla")
        {
            viewModel.ContentPages = _filterService.GetFilteredContentPages(viewModel.ContentPages, category);
            viewModel.SelectedCategory = category;
        }

        var model = await LayoutViewModel<ListingPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext);

        return View(model);
    }
}
