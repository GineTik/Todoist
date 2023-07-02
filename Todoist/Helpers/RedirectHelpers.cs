using Microsoft.AspNetCore.Mvc;
using Todoist.Controllers;

namespace Todoist.Helpers
{
    public static class RedirectHelpers
    {
        public static RedirectToActionResult RedirectBeforeSuccessAuthentication()
        {
            var controllerName = nameof(HomeController).Replace("Controller", "");
            return new RedirectToActionResult(nameof(HomeController.Index), controllerName, null, null);
        }
    }
}
