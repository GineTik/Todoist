using Microsoft.AspNetCore.Identity;
using Todoist.BusinessLogic.DTOs.User;
using Todoist.BusinessLogic.DTOs.User.Authentication;
using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.Services.Users.Authentication
{
    public interface IUserAuthenticationService
    {
        bool UserIsAuthenticated { get; }

        Task<ServiceResult> TryLoginAsync(LoginDTO dto);
        Task<ServiceResult> TryRegistrationAsync(RegistrationDTO dto);
        Task<ServiceResult> TryConfirmEmail(ConfirmEmailDTO dto);
        Task<ServiceResult> TryExternalLoginAsync(ExternalLoginInfo info);
        Task LogoutAsync();
        Task<UserDTO> GetAuthenticatedUserAsync();
        int GetAuthenticatedUserId();
    }
}
