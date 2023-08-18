using AutoMapper;
using Jalles.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Jalles.Web.Controllers;
public class ListingPageController : RenderControllerBase
{
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IMapper _mapper;
    private readonly IPaginationService _paginationService;

    public ListingPageController(ICompositeViewEngine compositeViewEngine, 
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        IPaginationService paginationService,
        ILogger<RenderController> logger) : base(compositeViewEngine, umbracoContextAccessor, logger)
    {
        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
        _paginationService = paginationService;
    }

    public async Task<IActionResult> IndexAsync(int page)
    {
        var listingPage = new ListingPage(CurrentPage, _publishedValueFallback);

        var viewModel = _mapper.Map<ListingPageViewModel>(listingPage);

        var model = await LayoutViewModel<ListingPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext);

        return View(model);
    }
}
