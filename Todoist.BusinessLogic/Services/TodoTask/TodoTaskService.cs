using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.BusinessLogic.Services.Boards;
using Todoist.Data.EF;
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

        public async Task<TodoTaskDTO> CreateAsync(CreateTaskDTO dto)
        {
            var task = _mapper.Map<TodoTask>(dto);

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoTaskDTO>(task);
        }

        public async Task<IEnumerable<TodoTaskDTO>> GetAllAsync(int boardId)
        {
            var tasks = await _context.Tasks
                .Where(task => task.BoardId == boardId)
                .ToListAsync();

            if (await _boardService.BoardBelongToAuthenticatedUserAsync(boardId) == false)
                throw new ArgumentException("Board not exists or user is not author");

            return _mapper.Map<IEnumerable<TodoTaskDTO>>(tasks);
        }

        public async Task RemoveAsync(int taskId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == taskId);

            if (await notNullAndCorrectAuthorAsync(task) == false)
                throw new ArgumentException("TaskId incorrect or user is not author");

            _context.Tasks.Remove(task!);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoTaskDTO> EditAsync(EditTaskDTO dto)
        {
            var task = await getTaskIfNotNullAndCorrectAuthorAsync(dto.TaskId);

            task.Name = dto.Name;
            task.Description = dto.Description;
            task.DateBeforeExpiration = dto.DateBeforeExpiration;

            return await updateTaskAsync(task);
        }

        public async Task<TodoTaskDTO> ToggleClosedAsync(int taskId)
        {
            var task = await getTaskIfNotNullAndCorrectAuthorAsync(taskId);
            
            task.IsClosed = !task.IsClosed;

            return await updateTaskAsync(task);
        }

        public async Task<TodoTaskDTO> EditPositionAsync(EditTaskPositionDTO dto)
        {
            var editedTask = await getTaskIfNotNullAndCorrectAuthorAsync(dto.NewPositions.First().TaskId);

            var editedTasks = await _context.Tasks
                .Where(task => task.BoardId == editedTask.BoardId)
                .ToDictionaryAsync(key => key.Id, value => value);

            foreach (var position in dto.NewPositions)
            {
                var task = editedTasks[position.TaskId] ??
                    throw new InvalidDataException($"You not have task with id {position.TaskId}");

                task.Position = position.NewPosition;
                _context.Tasks.Update(task);
            }

            return await updateTaskAsync(editedTask);
        }

        private async Task<TodoTask> getTaskIfNotNullAndCorrectAuthorAsync(int taskId)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == taskId);

            if (await notNullAndCorrectAuthorAsync(task) == false)
                throw new ArgumentException("TaskId incorrect or user is not author");

            return task!;
        }

        private async Task<TodoTaskDTO> updateTaskAsync(TodoTask? task)
        {
            _context.Tasks.Update(task!);
            await _context.SaveChangesAsync();
            return _mapper.Map<TodoTaskDTO>(task);
        }

        private async Task<bool> notNullAndCorrectAuthorAsync(TodoTask? task)
        {
            if (task == null)
                return false;

            return await _boardService.BoardBelongToAuthenticatedUserAsync(task!.BoardId);
        }
    }
}
