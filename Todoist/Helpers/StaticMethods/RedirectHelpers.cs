using Microsoft.AspNetCore.Mvc;
using Todoist.Controllers;

namespace Todoist.Helpers.StaticMethods
{
    public static class RedirectHelpers
    {
        public static RedirectToActionResult RedirectBeforeSuccessAuthentication()
        {
            var controllerName = nameof(BoardController).Replace("Controller", "");
            return new RedirectToActionResult(nameof(BoardController.Page), controllerName, new { page = 0, size = 10 }, null);
        }
    }
}
