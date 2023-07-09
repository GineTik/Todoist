using Microsoft.AspNetCore.Identity;
using Todoist.BusinessLogic.DTOs.User;
using Todoist.BusinessLogic.DTOs.User.Authentication;

namespace Todoist.BusinessLogic.Services.Users.Authentication
{
    public interface IUserAuthenticationService
    {
        bool UserIsAuthenticated { get; }

        Task<SignInResult> LoginAsync(LoginDTO dto);
        Task<IdentityResult> RegistrationAsync(RegistrationDTO dto);
        Task ExternalLoginAsync(ExternalLoginInfo info);
        Task LogoutAsync();
        Task<UserDTO> GetAuthenticatedUserAsync();
        Task<int> GetAuthenticatedUserIdAsync();
    }
}
