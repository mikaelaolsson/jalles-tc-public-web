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

    public SecondaryListingPageController(
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        IFilterService filterService,
        IContentAccessor contentAccessor,
        ILogger<RenderController> logger,
        ILayoutViewModelService layoutViewModelService)
        : base(compositeViewEngine, umbracoContextAccessor, logger, layoutViewModelService)
    {
        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
        _filterService = filterService;
        _contentAccessor = contentAccessor;
    }

    public async Task<IActionResult> IndexAsync()
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

        viewModel.ContentPages = _filterService.GetFilteredContentPages(contentPages, viewModel.MainCategory)
            .OrderByDescending(c => c.DateBlock?.PublishedDate ?? c.Published);

        var model = await LayoutViewModel<SecondaryListingPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext, LayoutViewModelService);

        return View(model);
    }
}
