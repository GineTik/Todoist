using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Todoist.BusinessLogic.DTOs.User;
using Todoist.BusinessLogic.DTOs.User.Authentication;
using Todoist.BusinessLogic.Services.Users;
using Todoist.BusinessLogic.Services.Users.Authentication;
using Todoist.Data.Models;
using Todoist.Helpers.StaticMethods;

namespace Todoist.Controllers
{
    public sealed class AuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authenticationService;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;

        public AuthenticationController(IUserAuthenticationService authenticationService, SignInManager<User> signInManager, IUserService userService)
        {
            _authenticationService = authenticationService;
            _signInManager = signInManager;
            _userService = userService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid == false)
                return View(dto);

            var result = await _authenticationService.LoginAsync(dto);

            IActionResult? actionResult = result switch
            {
                { Succeeded: true } => null,
                { IsNotAllowed: true } => confirmEmailView(dto.Email),
                _ => loginError("Email or password incorrect", dto),
            };

            if (actionResult != null)
                return actionResult;

            return RedirectHelpers.RedirectBeforeSuccessAuthentication();
        }

        public IActionResult Registration() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegistrationDTO dto)
        {
            if (ModelState.IsValid == false)
                return View(dto);

            var result = await _authenticationService.RegistrationAsync(dto);

            if (result.Succeeded == false)
            {
                addIdentityErrorsToModelStateErrors(result.Errors);
                return View(dto);
            }

            return confirmEmailView(dto.Email);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ExternalLogin(string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback));

            var properties =
                _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> ExternalLoginCallback(string? remoteError)
        {
            if (remoteError != null)
                return loginError($"Error from external provider: {remoteError}");

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return loginError("Error loading external login information.");

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (email == null)
                return loginError($"Email claim not received from: {info.LoginProvider}");

            await _authenticationService.ExternalLoginAsync(info);
            return RedirectHelpers.RedirectBeforeSuccessAuthentication();
        }

        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO dto)
        {
            var result = await _authenticationService.ConfirmEmail(dto);

            if (result.Succeeded == false)
                return View(dto);

            return RedirectHelpers.RedirectBeforeSuccessAuthentication();
        }

        private IActionResult loginError(string error, LoginDTO? dto = null)
        {
            ModelState.AddModelError(string.Empty, error);
            return View("Login", dto ?? ViewData.Model);
        }

        private void addIdentityErrorsToModelStateErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error.Description);
        }

        private ViewResult confirmEmailView(string email)
        {
            return View("ConfirmEmail", new ConfirmEmailDTO { Email = email });
        }
    }
}
