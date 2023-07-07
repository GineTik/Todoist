using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.Services.Boards;
using Todoist.Helpers.Extensions;

namespace Todoist.Controllers
{
    [Authorize]
    public sealed class BoardController : Controller
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService)
        {
            _boardService = boardService;
        }

        public async Task<IActionResult> All()
        {
            int userId = HttpContext.GetAuthenticationUserId();
            var boards = await _boardService.GetAllAsync(userId);
            return View(boards);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            var newBoard = await _boardService.CreateAsync(new CreateBoardDTO
            {
                Name = name,
                AuthorId = HttpContext.GetAuthenticationUserId()
            });

            return PartialView("_BoardItemPartial", newBoard);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _boardService.RemoveAsync(new RemoveBoardDTO
            {
                Id = id,
                AuthorId = HttpContext.GetAuthenticationUserId()
            });

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditName(int id, string name)
        {
            var editedBoard = await _boardService.EditNameAsync(new EditNameBoardDTO
            {
                BoardId = id,
                Name = name,
                AuthorId = HttpContext.GetAuthenticationUserId()
            });

            return PartialView("_BoardItemPartial", editedBoard);
        }
    }
}
