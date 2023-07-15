using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.Services.Boards;

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
            var boards = await _boardService.GetAllOfAuthenticatedUserAsync();
            return View(boards);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBoardDTO dto)
        {
            var newBoard = await _boardService.CreateAsync(dto);
            return PartialView("_BoardItemPartial", newBoard);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int boardId)
        {
            await _boardService.TryRemoveAsync(boardId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditName(EditNameBoardDTO dto)
        {
            var editedBoard = await _boardService.TryEditNameAsync(dto);
            return PartialView("_BoardItemPartial", editedBoard);
        }
    }
}
