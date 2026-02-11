using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Controllers;

namespace Jalles.Web.Controllers;
public class RenderControllerBase : RenderController
{
    protected RenderControllerBase(
        ICompositeViewEngine compositeViewEngine,
        IUmbracoContextAccessor umbracoContextAccessor,
        ILogger<RenderController> logger) : base(logger, compositeViewEngine, umbracoContextAccessor)
    {
    }

    [NonAction]
    public sealed override IActionResult Index() => throw new NotImplementedException();
}
