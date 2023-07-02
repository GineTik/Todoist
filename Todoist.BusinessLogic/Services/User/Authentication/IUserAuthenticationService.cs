using Microsoft.AspNetCore.Identity;
using Todoist.BusinessLogic.DTOs.User.Authentication;

namespace Todoist.BusinessLogic.Services.Users.Authentication
{
    public interface IUserAuthenticationService
    {
        Task<SignInResult> LoginAsync(LoginDTO dto);
        Task<IdentityResult> RegistrationAsync(RegistrationDTO dto);
        Task LogoutAsync();
    }
}
