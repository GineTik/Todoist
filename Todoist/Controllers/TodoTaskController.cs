using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.BusinessLogic.Services.Boards;
using Todoist.BusinessLogic.Services.TodoTasks;

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
        public async Task<IActionResult> Create(CreateTaskDTO dto)
        {
            var createdTask = await _taskService.CreateAsync(dto);
            return PartialView("_TaskItemPartial", createdTask);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int taskId)
        {
            await _taskService.RemoveAsync(taskId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTaskDTO dto)
        {
            var editedTask = await _taskService.EditAsync(dto);
            return PartialView("_TaskItemPartial", editedTask);
        }
    }
}
