using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Jalles.Web.Controllers;
public class ListingPageController : RenderControllerBase
{
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IMapper _mapper;

    public ListingPageController(ICompositeViewEngine compositeViewEngine, 
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        ILogger<RenderController> logger) : base(compositeViewEngine, umbracoContextAccessor, logger)
    {
        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var listingPage = new ListingPage(CurrentPage, _publishedValueFallback);

        var viewModel = _mapper.Map<ListingPageViewModel>(listingPage);

        var model = await LayoutViewModel<ListingPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext);

        return View(model);
    }
}
