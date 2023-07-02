using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.User.Authentication;
using Todoist.BusinessLogic.Services.Users.Authentication;
using Todoist.Helpers;

namespace Todoist.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authenticationService;

        public AuthenticationController(IUserAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (ModelState.IsValid == false)
            {
                return View(dto);
            }

            var result = await _authenticationService.LoginAsync(dto);

            if (result.Succeeded == false)
            {
                ModelState.AddModelError("", "Email or password invalid");
                return View(dto);
            }

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
            {
                return View(dto);
            }

            var result = await _authenticationService.RegistrationAsync(dto);

            if (result.Succeeded == false)
            {
                addIdentityErrorsToModelStateErrors(result.Errors);
                return View(dto);
            }

            return RedirectHelpers.RedirectBeforeSuccessAuthentication();
        }
         
        public async Task<IActionResult> Logout()
        {
            await _authenticationService.LogoutAsync();
            return Redirect("");
        }

        private void addIdentityErrorsToModelStateErrors(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}
