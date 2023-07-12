using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.Services.Users;

namespace Todoist.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveAccount()
        {
            await _userService.RemoveAuthenticatedAccountAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
