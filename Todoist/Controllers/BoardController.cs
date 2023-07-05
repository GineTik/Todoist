using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.Services.Boards;
using Todoist.Data.Models;

namespace Todoist.Controllers
{
    [Authorize]
    public sealed class BoardController : Controller
    {
        private readonly IBoardService _boardService;
        private readonly UserManager<User> _userManager;

        public BoardController(IBoardService boardService, UserManager<User> userManager)
        {
            _boardService = boardService;
            _userManager = userManager;
        }

        public int UserId => _userManager.GetUserAsync(HttpContext.User).Result!.Id;

        public async Task<IActionResult> All()
        {
            var boards = await _boardService.GetAllAsync(UserId);
            return View(boards);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var newBoard = await _boardService.CreateAsync(new CreateBoardDTO
            {
                Name = name,
                AuthorId = UserId
            });

            return PartialView("_BoardItemPartial", newBoard);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _boardService.RemoveAsync(new RemoveBoardDTO
            {
                Id = id,
                UserId = UserId
            });

            return Ok();
        }
    }
}
