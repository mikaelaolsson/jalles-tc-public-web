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

    public SecondaryListingPageController(
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        IFilterService filterService,
        IContentAccessor contentAccessor,
        IPaginationService paginationService,
        ILogger<RenderController> logger,
        ILayoutViewModelService layoutViewModelService)
        : base(compositeViewEngine, umbracoContextAccessor, logger, layoutViewModelService)
    {
        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
        _filterService = filterService;
        _contentAccessor = contentAccessor;
        _paginationService = paginationService;
    }

    public async Task<IActionResult> IndexAsync(List<string> categories, int page)
    {
        var secondaryListingPage = new SecondaryListingPage(CurrentPage, _publishedValueFallback);
        var viewModel = _mapper.Map<SecondaryListingPageViewModel>(secondaryListingPage);

        var listingPage = _contentAccessor.GetChildrenOfType<StartPage, ListingPage>().FirstOrDefault();

        IEnumerable<ContentPage> children = [];
        if(listingPage != null)
        {
            children = _contentAccessor.GetChildrenOfTypeFromParent<ContentPage>(listingPage);
        }

        var contentPages = _mapper.Map<IEnumerable<ContentPageViewModel>>(children) ?? [];

        viewModel.ContentPages = _filterService.GetFilteredContentPages(contentPages, viewModel.MainCategory).OrderByDescending(c => c.DateBlock?.PublishedDate ?? c.Published);

        if(categories.Any(c => !string.IsNullOrWhiteSpace(c)))
        {
            var category = categories.Find(c => !string.IsNullOrWhiteSpace(c));

            viewModel.ContentPages = _filterService.GetFilteredContentPages(viewModel.ContentPages, category);
            viewModel.SelectedCategory = category ?? "Alla";
        }

        viewModel = _paginationService.GetPaginatedViewModel(viewModel, page <= 0 ? 1 : page);

        var model = await LayoutViewModel<SecondaryListingPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext, LayoutViewModelService);

        return View(model);
    }
}
