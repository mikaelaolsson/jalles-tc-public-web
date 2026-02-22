using AutoMapper;
using Jalles.Core.Contracts;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Jalles.Web.Controllers;

public class ErrorPageController : RenderControllerBase
{
    private readonly IPublishedValueFallback _publishedValueFallback;
    private readonly IMapper _mapper;

    public ErrorPageController(
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedValueFallback publishedValueFallback,
        IMapper mapper,
        ILogger<RenderController> logger,
        ILayoutViewModelService layoutViewModelService)
        : base(compositeViewEngine, umbracoContextAccessor, logger, layoutViewModelService)
    {
        _publishedValueFallback = publishedValueFallback;
        _mapper = mapper;
    }

    public async Task<IActionResult> IndexAsync()
    {
        var errorPage = new ErrorPage(CurrentPage, _publishedValueFallback);

        var viewModel = _mapper.Map<ErrorPageViewModel>(errorPage);

        var model = await LayoutViewModel<ErrorPageViewModel>.CreateAsync(viewModel, CurrentPage!, HttpContext, LayoutViewModelService);

        return View(model);
    }
}
