using AutoMapper;
using Jalles.Core.Contracts;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Jalles.Web.Controllers;
public class SecondaryListingPageController : RenderControllerBase
{

    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IMapper _mapper;
    private readonly IFilterService _filterService;
    private readonly IContentAccessor _contentAccessor;
    private readonly IPaginationService _paginationService;

    public SecondaryListingPageController(ICompositeViewEngine compositeViewEngine, 
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        IFilterService filterService,
        IContentAccessor contentAccessor,
        IPaginationService paginationService,
        ILogger<RenderController> logger) : base(compositeViewEngine, umbracoContextAccessor, logger)
    {

        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
        _filterService = filterService;
        _contentAccessor = contentAccessor;
        _paginationService = paginationService;
    }

    public async Task<IActionResult> IndexAsync(string category, int page, string? selectedCategory)
    {
        var secondaryListingPage = new SecondaryListingPage(CurrentPage, _publishedValueFallback);

        var viewModel = _mapper.Map<SecondaryListingPageViewModel>(secondaryListingPage);

        var listingPage = _contentAccessor.GetLandingPage<ListingPage>();

        var contentPages = _mapper.Map<IEnumerable<ContentPageViewModel>>(listingPage?.Children) ?? Enumerable.Empty<ContentPageViewModel>();

        viewModel.ContentPages = _filterService.GetFilteredContentPages(contentPages, viewModel.MainCategory);

        if (!string.IsNullOrEmpty(category) && category != "Alla")
        {
            viewModel.ContentPages = _filterService.GetFilteredContentPages(viewModel.ContentPages, category);
            viewModel.SelectedCategory = category;
        }
        else if (!string.IsNullOrEmpty(selectedCategory) && selectedCategory != "Alla")
        {
            viewModel.ContentPages = _filterService.GetFilteredContentPages(viewModel.ContentPages, selectedCategory);
            viewModel.SelectedCategory = selectedCategory;
        }
        viewModel = _paginationService.GetPaginatedViewModel(viewModel, page <= 0 ? 1 : page);

        var model = await LayoutViewModel<SecondaryListingPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext);

        return View(model);
    }
}