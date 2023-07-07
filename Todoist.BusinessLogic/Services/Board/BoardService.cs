using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.Data.EF;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.Services.Boards
{
    public class BoardService : IBoardService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public BoardService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> BoardBelongToUserAsync(BoardBelongToUserDTO dto)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(board => board.Id == dto.BoardId);
            return boardBelongToUser(board, dto.UserId);
        }

        public async Task<BoardDTO> CreateAsync(CreateBoardDTO dto)
        {
            var newBoard = _mapper.Map<Board>(dto);

            _context.Boards.Add(newBoard);
            await _context.SaveChangesAsync();

            return _mapper.Map<BoardDTO>(newBoard);
        }

        public async Task<BoardDTO> EditNameAsync(EditNameBoardDTO dto)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(board => board.Id == dto.BoardId);

            if (boardBelongToUser(board, dto.AuthorId) == false)
                throw new ArgumentException("Board not exists or user is not author");

            board!.Name = dto.Name;
            var result = _context.Boards.Update(board);
            await _context.SaveChangesAsync();

            return _mapper.Map<BoardDTO>(result.Entity);
        }

        public async Task<IEnumerable<BoardDTO>> GetAllAsync(int userId)
        {
            return await _context.Boards
                .Where(board => board.AuthorId == userId)
                .Select(board => _mapper.Map<BoardDTO>(board))
                .ToListAsync();
        }

        public async Task<BoardWithTasksDTO?> GetAsync(int boardId)
        {
            var board = await _context.Boards
                .Include(board => board.Tasks)
                .FirstOrDefaultAsync(board => board.Id == boardId);

            return _mapper.Map<BoardWithTasksDTO>(board);
        }

        public async Task RemoveAsync(RemoveBoardDTO dto)
        {
            var boardToRemove = await _context.Boards
                .FirstOrDefaultAsync(board => board.Id == dto.Id);

            if (boardBelongToUser(boardToRemove, dto.AuthorId) == false)
                throw new ArgumentException("Board not exists or user is not author");

            _context.Boards.Remove(boardToRemove);
            await _context.SaveChangesAsync();
        }

        private bool boardBelongToUser(Board? board, int userId)
        {
            return board?.AuthorId == userId;
        }
    }
}
