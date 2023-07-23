using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.DTOs.Page;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.BusinessLogic.ServiceResults.Base;
using Todoist.BusinessLogic.ServiceResults.TodoTasks;
using Todoist.BusinessLogic.Services.Boards;
using Todoist.Data.EF;
using Todoist.Data.EF.Extensions;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.Services.TodoTasks
{
    public sealed class TodoTaskService : ITodoTaskService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IBoardService _boardService;

        public TodoTaskService(DataContext context, IMapper mapper, IBoardService boardService)
        {
            _context = context;
            _mapper = mapper;
            _boardService = boardService;
        }

        public async Task<ServiceValueResult<TodoTaskDTO>> TryCreateAsync(CreateTaskDTO dto)
        {
            var task = _mapper.Map<TodoTask>(dto);
            task.Position = int.MaxValue;

            if (await notNullAndCorrectAuthorAsync(task) == false)
                return TodoTaskResult.TaskNotBelongUser;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var taskDTO = _mapper.Map<TodoTaskDTO>(task);
            return TodoTaskResult.Success(taskDTO);
        }

        public async Task<ServiceValueResult<IEnumerable<TodoTaskDTO>>> TryGetAllAsync(int boardId)
        {
            var tasks = await _context.Tasks
                .Where(task => task.BoardId == boardId)
                .ToListAsync();

            if (await _boardService.BoardBelongToAuthenticatedUserAsync(boardId) == false)
                return TodoTasksResult.BoardNotBelongUser;

            var tasksDTOs = _mapper.Map<IEnumerable<TodoTaskDTO>>(tasks);
            return TodoTasksResult.Success(tasksDTOs);
        }

        public async Task<ServiceResult> TryRemoveAsync(int taskId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == taskId);

            if (await notNullAndCorrectAuthorAsync(task) == false)
                return TodoTaskResult.TaskNotBelongUser;

            _context.Tasks.Remove(task!);
            await _context.SaveChangesAsync();
            return ServiceResult.Success;
        }

        public async Task<ServiceValueResult<TodoTaskDTO>> TryEditAsync(EditTaskDTO dto)
        {
            return await editAsync(dto.TaskId, async task =>
            {
                task.Name = dto.Name;
                task.Description = dto.Description;
                task.DateBeforeExpiration = dto.DateBeforeExpiration;
                await Task.CompletedTask;
            });
        }

        public async Task<ServiceValueResult<TodoTaskDTO>> TryToggleClosedAsync(int taskId)
        {
            return await editAsync(taskId, async task =>
            {
                task.IsClosed = !task.IsClosed;
                await Task.CompletedTask;
            });
        }

        public async Task<ServiceValueResult<TodoTaskDTO>> TryEditPositionAsync(EditTaskPositionDTO dto)
        {
            return await editAsync(dto.NewPositions.First().TaskId, async _ =>
            {
                var tasksToEdit = await _context.Tasks
                    .Where(task => task.BoardId == task.BoardId)
                    .ToDictionaryAsync(key => key.Id, value => value);

                foreach (var position in dto.NewPositions)
                {
                    var taskToEdit = tasksToEdit[position.TaskId] ??
                        throw new InvalidDataException($"You not have task with id {position.TaskId}");

                    taskToEdit.Position = position.NewPosition;
                    _context.Tasks.Update(taskToEdit);
                }
            });
        }

        private async Task<ServiceValueResult<TodoTaskDTO>> editAsync(int taskId, Func<TodoTask, Task> editing)
        {
            ArgumentNullException.ThrowIfNull(editing);

            var task = await _context.Tasks
               .FirstOrDefaultAsync(task => task.Id == taskId);

            if (await notNullAndCorrectAuthorAsync(task) == false)
                return TodoTaskResult.TaskNotBelongUser;

            await editing.Invoke(task!);

            _context.Tasks.Update(task!);
            await _context.SaveChangesAsync();

            var taskDTO = _mapper.Map<TodoTaskDTO>(task);
            return TodoTaskResult.Success(taskDTO);
        }

        private async Task<ServiceValueResult<TodoTaskDTO>> updateTaskAsync(TodoTask? task)
        {
            _context.Tasks.Update(task!);
            await _context.SaveChangesAsync();
            var taskDTO = _mapper.Map<TodoTaskDTO>(task);
            return TodoTaskResult.Success(taskDTO);
        }

        private async Task<bool> notNullAndCorrectAuthorAsync(TodoTask? task)
        {
            if (task == null)
                return false;

            return await _boardService.BoardBelongToAuthenticatedUserAsync(task!.BoardId);
        }
    }
}
