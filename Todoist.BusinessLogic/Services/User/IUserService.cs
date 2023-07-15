using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.Services.Users
{
    public interface IUserService
    {
        Task<ServiceResult> TryRemoveAuthenticatedAccountAsync();
    }
}
