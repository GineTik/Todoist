using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;
using System.Security.Claims;
using Todoist.BusinessLogic.DTOs.User;
using Todoist.BusinessLogic.DTOs.User.Authentication;
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

        public async Task<SignInResult> LoginAsync(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Email);
            if (user == null)
                return SignInResult.Failed;

            var result = await _signInManager.PasswordSignInAsync(
                user,
                dto.Password,
                REMEMBER_ME,
                lockoutOnFailure: false
            );
            
            if (result.Succeeded == true && user.EmailConfirmed == false)
            {
                await sendConfirmationEmail(user);
                return SignInResult.NotAllowed;
            }

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
                await sendConfirmationEmail(user);

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
                await createUserWithoutPassword(email);

            await _userManager.AddLoginAsync(user, info);
            await _signInManager.SignInAsync(user, isPersistent: REMEMBER_ME);
        }

        public async Task<UserDTO> GetAuthenticatedUserAsync()
        {
            if (UserIsAuthenticated == false)
                throw new AuthenticationException("User not authentication");

            var user = await _userManager.GetUserAsync(_httpContext.User);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<int> GetAuthenticatedUserIdAsync()
        {
            var user = await GetAuthenticatedUserAsync();
            return user.Id;
        }

        public async Task<IdentityResult> ConfirmEmail(ConfirmEmailDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email) ??
                throw new ArgumentException("User not found");

            var result = await _userManager.ConfirmEmailAsync(user, dto.Code);

            if (result.Succeeded == true)
                await _signInManager.SignInAsync(user, REMEMBER_ME);

            return result;
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
