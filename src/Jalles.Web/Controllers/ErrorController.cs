using Microsoft.AspNetCore.Mvc;

namespace Jalles.Web.Controllers;
public class ErrorController : Controller
{
    [Route("error")]
    public IActionResult Index()
    {
        return View();
    }
}
