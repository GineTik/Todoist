using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.Data.EF;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.Services.TodoTasks
{
    public sealed class TodoTaskService : ITodoTaskService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TodoTaskService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

            if (board?.AuthorId != dto.AuthorId)
                throw new ArgumentException("User is not author");

            return board.Tasks
                .Select(_mapper.Map<TodoTaskDTO>);
        }

        public async Task RemoveAsync(RemoveTaskDTO dto)
        {
            var task = await _context.Tasks
                .Include(task => task.Board)
                .FirstOrDefaultAsync(task => task.Id == dto.TaskId);

            if (task == null)
                throw new ArgumentOutOfRangeException("TaskId incorrect");

            if (task.Board.AuthorId != dto.AuthorId)
                throw new ArgumentException("User is not author");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }
    }
}
