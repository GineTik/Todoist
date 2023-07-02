using Microsoft.AspNetCore.Mvc;

namespace Todoist.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
