using Microsoft.AspNetCore.Identity;
using Todoist.BusinessLogic.ServiceResults.Authentication;
using Todoist.BusinessLogic.ServiceResults.Base;
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

        public async Task<ServiceResult> TryRemoveAuthenticatedAccountAsync()
        {
            if (_authenticationService.UserIsAuthenticated == false)
                return AuthenticationResult.UserNotAuthenticated;

            var userId = _authenticationService.GetAuthenticatedUserId();
            await _authenticationService.LogoutAsync();

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                throw new ArgumentException("User not found"); // it is throw if client change the id in the cockie

            var logins = await _userManager.GetLoginsAsync(user);
            foreach (var login in logins)
                await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);

            await _userManager.DeleteAsync(user);
            return AuthenticationResult.Success;
        }
    }
}
