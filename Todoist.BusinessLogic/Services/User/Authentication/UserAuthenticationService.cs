using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Security.Claims;
using Todoist.BusinessLogic.DTOs.User;
using Todoist.BusinessLogic.DTOs.User.Authentication;
using Todoist.BusinessLogic.ServiceResults.Authentication;
using Todoist.BusinessLogic.ServiceResults.Base;
using Todoist.BusinessLogic.Services.Email;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.Services.Users.Authentication
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly HttpContext _httpContext;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        private const bool REMEMBER_ME = true;

        public UserAuthenticationService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContextAccessor.HttpContext;
            _mapper = mapper;
            _emailService = emailService;
        }

        public bool UserIsAuthenticated => _httpContext.User.Identity?.IsAuthenticated ?? false;

        public async Task<ServiceResult> TryLoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
                return LoginResult.EmailOrPasswordIncorrect;

            var result = await _signInManager.PasswordSignInAsync(
                user,
                dto.Password,
                REMEMBER_ME,
                lockoutOnFailure: false
            );
            
            if (result.Succeeded == true && user.EmailConfirmed == false)
            {
                await sendConfirmationEmail(user);
                return LoginResult.EmailNotConfirmed;
            }

            if (result.ToString() == "Failed")
                return LoginResult.EmailOrPasswordIncorrect;
            else if (result.Succeeded == false)
                return LoginResult.SignInResult(result);

            return LoginResult.Success;
        }

        public async Task<ServiceResult> TryRegistrationAsync(RegistrationDTO dto)
        {
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.Email,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded == true)
                await sendConfirmationEmail(user);

            return AuthenticationResult.IdentityResult(result);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ServiceResult> TryExternalLoginAsync(ExternalLoginInfo info)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: REMEMBER_ME, bypassTwoFactor: true);

            if (signInResult.Succeeded == true)
                return ExternalLoginResult.Success;

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return ExternalLoginResult.EmailNotFound;

            var user =
                await _userManager.FindByEmailAsync(email) ??
                await createUserWithoutPassword(email);

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: REMEMBER_ME);
            return ExternalLoginResult.Success;
        }

        public async Task<UserDTO> GetAuthenticatedUserAsync()
        {
            if (UserIsAuthenticated == false)
                throw new AuthenticationException("User not authentication");

            var user = await _userManager.GetUserAsync(_httpContext.User);
            return _mapper.Map<UserDTO>(user);
        }

        public int GetAuthenticatedUserId()
        {
            if (UserIsAuthenticated == false)
                throw new AuthenticationException("User not authentication");

            var id = _userManager.GetUserId(_httpContext.User);
            return int.Parse(id!);
        }

        public async Task<ServiceResult> TryConfirmEmail(ConfirmEmailDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email) ??
                throw new ArgumentException("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, dto.Code);

            if (result.Succeeded == true)
                await _signInManager.SignInAsync(user, REMEMBER_ME);

            return AuthenticationResult.IdentityResult(result);
        }

        private async Task<User> createUserWithoutPassword(string email)
        {
            var user = new User
            {
                UserName = email,
                Email = email
            };

            await _userManager.CreateAsync(user);
            return user;
        }

        private async Task sendConfirmationEmail(User user)
        {
            var confirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendConfirmationEmailAsync(user.Email!, confirmationCode);
        }
    }
}
