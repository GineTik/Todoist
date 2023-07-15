using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.User.Authentication;
using Todoist.BusinessLogic.ServiceResults.Authentication;
using Todoist.BusinessLogic.ServiceResults.Base;
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

            var result = await _authenticationService.TryLoginAsync(dto);

            if (result == LoginResult.EmailNotConfirmed)
                return confirmEmailView(dto.Email);
            
            if (result.Successfully == false)
                return authenticationError(result);

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

            var result = await _authenticationService.TryRegistrationAsync(dto);

            if (result.Successfully == false)
                return authenticationError(result);

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

            var result = await _authenticationService.TryExternalLoginAsync(info);

            if (result.Successfully == false)
                return authenticationError(result);

            return RedirectHelpers.RedirectBeforeSuccessAuthentication();
        }

        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO dto)
        {
            var result = await _authenticationService.TryConfirmEmail(dto);

            if (result.Successfully == false)
                return authenticationError(result);

            return RedirectHelpers.RedirectBeforeSuccessAuthentication();
        }

        private IActionResult authenticationError(ServiceResult result)
        {
            foreach (var error in result.ErrorCodes)
                ModelState.AddModelError(string.Empty, error);

            return View();
        }

        private IActionResult loginError(string error)
        {
            ModelState.AddModelError(string.Empty, error);
            return View("Login");
        }

        private ViewResult confirmEmailView(string email)
        {
            return View("ConfirmEmail", new ConfirmEmailDTO { Email = email });
        }
    }
}
