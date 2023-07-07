using Microsoft.AspNetCore.Mvc;

namespace Todoist.Helpers.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string? ActionLink<TConroller>(this IUrlHelper helper, string action)
            where TConroller : Controller
        {
            var controllerName = typeof(TConroller).Name;
            var controllerNameLenght = controllerName.Length - "Controller".Length;
            var controllerNameWithoutControllerWord = controllerName.Substring(0, controllerNameLenght);

            return helper.ActionLink(action, controllerNameWithoutControllerWord);
        }
    }
}
