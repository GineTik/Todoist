using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == dto.TaskId);

            if (await notNullAndCorrectAuthorAsync(task) == false)
                throw new ArgumentException("TaskId incorrect or user is not author");

            task!.Name = dto.Name;
            task.Description = dto.Description; 
            task.ClosingDate = dto.ClosingDate;

            _context.Tasks.Update(task);
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
