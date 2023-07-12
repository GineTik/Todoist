using Microsoft.AspNetCore.Identity;
using Todoist.BusinessLogic.Services.Users.Authentication;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserAuthenticationService _authenticationService;

        public UserService(
            UserManager<User> userManager, 
            IUserAuthenticationService authenticationService)
        {
            _userManager = userManager;
            _authenticationService = authenticationService;
        }

        public async Task RemoveAuthenticatedAccountAsync()
        {
            var userId = await _authenticationService.GetAuthenticatedUserIdAsync();
            await _authenticationService.LogoutAsync();

            var user = await _userManager.FindByIdAsync(userId.ToString()) ??
                throw new ArgumentException("User not found");

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var login in logins)
                await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);

            await _userManager.DeleteAsync(user);
        }
    }
}
