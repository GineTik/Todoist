using System.Security.Claims;

namespace Todoist.Helpers.Extensions
{
    public static class HttpContextExtension
    {
        public static int GetAuthenticationUserId(this HttpContext httpContext)
        {
            ArgumentNullException.ThrowIfNull(httpContext);

            if (httpContext.User.Identity?.IsAuthenticated == false)
                throw new InvalidOperationException("User not authenticated");

            var stringId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier.ToString());
            if (stringId == null)
                throw new InvalidOperationException("NameIdentifier claim not found");

            return int.Parse(stringId);
        }
    }
}
