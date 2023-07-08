using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Todoist.BusinessLogic.DTOs.User.Authentication;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.Services.Users.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private const bool REMEMBER_ME = true;

        public UserAuthenticationService(
            IUserService userService, 
            UserManager<User> userManager, 
            SignInManager<User> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(
                dto.Email, 
                dto.Password, 
                REMEMBER_ME, 
                lockoutOnFailure: false
            );
            return result;
        }

        public async Task<IdentityResult> RegistrationAsync(RegistrationDTO dto)
        {
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded == true)
                await _signInManager.SignInAsync(user, isPersistent: REMEMBER_ME);

            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task ExternalLoginAsync(ExternalLoginInfo info)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: REMEMBER_ME, bypassTwoFactor: true);

            if (signInResult.Succeeded == true)
                return;

            var email = info.Principal.FindFirstValue(ClaimTypes.Email) ??
                throw new InvalidOperationException($"Email claim not received from: {info.LoginProvider}");

            var user = 
                await _userManager.FindByEmailAsync(email) ??
                await registrationWithoutPassword(email);

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: REMEMBER_ME);
        }

        private async Task<User> registrationWithoutPassword(string email)
        {
            var user = new User
            {
                UserName = email,
                Email = email
            };

            await _userManager.CreateAsync(user);
            return user;
        }
    }
}
