using Microsoft.AspNetCore.Identity;
using Todoist.BusinessLogic.DTOs.User;
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
            //var userExists = await _userService.UserExistsAsync(dto.Email);

            //if (userExists == false)
            //    return IdentityResult.Failed(;

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
    }
}
