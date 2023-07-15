using Microsoft.AspNetCore.Identity;
using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.ServiceResults.Authentication
{
    public class AuthenticationResult : ServiceResult
    {
        public static ServiceResult IdentityResult(IdentityResult result)
        {
            if(result.Succeeded == false)
                return Error(result.Errors.Select(r => r.Code));
            else
                return Success;
        }

        public static ServiceResult SignInResult(SignInResult result)
        {
            return Error(result.ToString());
        }

        public static ServiceResult UserNotAuthenticated => Error("UserNotAuthenticated");
    }
}
