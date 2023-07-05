using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.BusinessLogic.Services.Boards;
using Todoist.BusinessLogic.Services.TodoTasks;
using Todoist.Helpers.Extensions;

namespace Todoist.Controllers
{
    [Authorize]
    public sealed class TodoTaskController : Controller
    {
        private readonly ITodoTaskService _taskService;
        private readonly IBoardService _boardService;

        public TodoTaskController(ITodoTaskService taskService, IBoardService boardService)
        {
            _taskService = taskService;
            _boardService = boardService;
        }

        public async Task<IActionResult> All(int boardId)
        {
            var boardWithTasks = await _boardService.GetAsync(boardId);
            return View(boardWithTasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name, int boardId)
        {
            var task = await _taskService.CreateAsync(new CreateTaskDTO
            {
                Title = name,
                Description = "",
                BoardId = boardId,
                ClosingTime = DateTime.UtcNow
            });
            return PartialView("_TaskItemPartial", task);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int taskId)
        {
            await _taskService.RemoveAsync(new RemoveTaskDTO
            {
                TaskId = taskId,
                AuthorId = HttpContext.GetAuthenticationUserId()
            });
            return Ok();
        }
    }
}
