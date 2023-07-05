using Microsoft.AspNetCore.Mvc;
using Todoist.Helpers.StaticMethods;

namespace Todoist.Controllers
{
    public sealed class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.User.Identity?.IsAuthenticated == true)      
                return RedirectHelpers.RedirectBeforeSuccessAuthentication();

            return View();
        }
    }
}
