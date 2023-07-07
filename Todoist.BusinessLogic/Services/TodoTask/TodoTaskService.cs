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

        public async Task<IEnumerable<TodoTaskDTO>> GetAllAsync(GetTaskDTO dto)
        {
            var board = await _context.Boards
                .Include(board => board.Tasks)
                .FirstOrDefaultAsync(board => board.Id == dto.BoardId);

            if (await _boardService.BoardBelongToUserAsync(new() { BoardId = dto.BoardId, UserId = dto.AuthorId }) == false)
                throw new ArgumentException("Board not exists or user is not author");

            return board!.Tasks
                .Select(_mapper.Map<TodoTaskDTO>);
        }

        public async Task RemoveAsync(RemoveTaskDTO dto)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == dto.TaskId);

            if (await notNullAndCorrectAuthorAsync(dto.AuthorId, task) == false)
                throw new ArgumentException("TaskId incorrect or user is not author");

            _context.Tasks.Remove(task!);
            await _context.SaveChangesAsync();
        }

        public async Task<TodoTaskDTO> EditAsync(EditTaskDTO dto)
        {
            var task = await _context.Tasks
                .FirstOrDefaultAsync(task => task.Id == dto.TaskId);

            if (await notNullAndCorrectAuthorAsync(dto.AuthorId, task) == false)
                throw new ArgumentException("TaskId incorrect or user is not author");

            task!.Title = dto.Title;
            task.Description = dto.Description; 
            task.ClosingDate = dto.ClosingDate;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return _mapper.Map<TodoTaskDTO>(task);
        }

        private async Task<bool> notNullAndCorrectAuthorAsync(int authorId, TodoTask? task)
        {
            if (task == null)
                return false;

            return await _boardService.BoardBelongToUserAsync(new()
            {
                BoardId = task!.BoardId,
                UserId = authorId
            });
        }
    }
}
