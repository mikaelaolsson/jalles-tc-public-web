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
    private readonly IPaginationService _paginationService;
    private readonly IFilterService _filterService;

    public ListingPageController(
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        IPaginationService paginationService,
        IFilterService filterService,
        ILogger<RenderController> logger,
        ILayoutViewModelService layoutViewModelService)
        : base(compositeViewEngine, umbracoContextAccessor, logger, layoutViewModelService)
    {
        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
        _paginationService = paginationService;
        _filterService = filterService;
    }

    public async Task<IActionResult> IndexAsync(List<string> categories, int page)
    {
        var listingPage = new ListingPage(CurrentPage, _publishedValueFallback);

        var viewModel = _mapper.Map<ListingPageViewModel>(listingPage);
        viewModel.ContentPages = viewModel.ContentPages.OrderByDescending(c => c.DateBlock?.PublishedDate ?? c.Published);

        if(categories.Any(c => !string.IsNullOrWhiteSpace(c)))
        {
            var category = categories.Find(c => !string.IsNullOrWhiteSpace(c));

            viewModel.ContentPages = _filterService.GetFilteredContentPages(viewModel.ContentPages, category);
            viewModel.SelectedCategory = category ?? "Alla";
        }

        viewModel = _paginationService.GetPaginatedViewModel(viewModel, page <= 0 ? 1 : page);

        var model = await LayoutViewModel<ListingPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext, LayoutViewModelService);

        return View(model);
    }
}
