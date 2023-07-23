using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.BusinessLogic.ServiceResults.Base;
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
            var boardWithTasks = await _boardService.GetWithTasksAsync(boardId);
            if (boardWithTasks == null)
                return NotFound();

            return View(boardWithTasks);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDTO dto)
        {
            var result = await _taskService.TryCreateAsync(dto);

            if (result.Successfully == false)
                return StatusCode(500, result);

            return PartialView("_TaskItemPartial", result.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int taskId)
        {
            var result = await _taskService.TryRemoveAsync(taskId);

            if (result.Successfully == false)
                return StatusCode(500, result);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTaskDTO dto)
        {
            return await editAsync(async () => await _taskService.TryEditAsync(dto));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleClosedValue(int taskId)
        {
            return await editAsync(async () => await _taskService.TryToggleClosedAsync(taskId));
        }

        [HttpPost]
        public async Task<IActionResult> EditPosition(EditTaskPositionDTO dto)
        {
            return await editAsync(async () => await _taskService.TryEditPositionAsync(dto));
        }

        private async Task<IActionResult> editAsync(Func<Task<ServiceValueResult<TodoTaskDTO>>> getEditedTask)
        {
            var result = await getEditedTask();

            if (result.Successfully == false)
                return StatusCode(500, result);

            return PartialView("_TaskItemPartial", result.Result);
        }
    }
}
