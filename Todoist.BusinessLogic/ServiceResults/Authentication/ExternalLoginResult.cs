using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.ServiceResults.Authentication
{
    public class ExternalLoginResult : ServiceResult
    {
        public static ServiceResult EmailNotFound => new ServiceResult("EmailNotFoundInThisProvider");
    }
}
