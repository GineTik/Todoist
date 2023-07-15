using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.ServiceResults.Authentication
{
    public sealed class LoginResult : AuthenticationResult
    {
        public static ServiceResult EmailOrPasswordIncorrect => new ServiceResult("EmailOrPasswordIncorrect");
        public static ServiceResult EmailNotConfirmed => new ServiceResult("EmailNotConfirmed");
    }
}
