using Jalles.Core.Contracts;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Jalles.Web.Controllers;

public class RenderControllerBase : RenderController
{
    protected readonly ILayoutViewModelService LayoutViewModelService;

    protected RenderControllerBase(
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        ILogger<RenderController> logger,
        ILayoutViewModelService layoutViewModelService) : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
        LayoutViewModelService = layoutViewModelService;
    }

    [NonAction]
    public sealed override IActionResult Index() => throw new NotImplementedException();
}
