using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.Services.Users.Authentication;
using Todoist.Data.EF;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.Services.Boards
{
    public class BoardService : IBoardService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAuthenticationService _authenticationService;

        public BoardService(DataContext context, IMapper mapper, IUserAuthenticationService authenticationService)
        {
            _context = context;
            _mapper = mapper;
            _authenticationService = authenticationService;
        }

        public async Task<bool> BoardBelongToAuthenticatedUserAsync(int boardId)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(board => board.Id == boardId);
            return await boardBelongToAuthenticatedUserAsync(board);
        }

        public async Task<BoardDTO> CreateAsync(CreateBoardDTO dto)
        {
            var newBoard = _mapper.Map<Board>(dto);
            newBoard.AuthorId = await _authenticationService.GetAuthenticatedUserIdAsync();

            _context.Boards.Add(newBoard);
            await _context.SaveChangesAsync();

            return _mapper.Map<BoardDTO>(newBoard);
        }

        public async Task<BoardDTO> EditNameAsync(EditNameBoardDTO dto)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(board => board.Id == dto.BoardId);

            if (await boardBelongToAuthenticatedUserAsync(board) == false)
                throw new ArgumentException("Board not exists or user is not author");

            board!.Name = dto.Name;
            var result = _context.Boards.Update(board);
            await _context.SaveChangesAsync();

            return _mapper.Map<BoardDTO>(result.Entity);
        }

        public async Task<IEnumerable<BoardDTO>> GetAllOfAuthenticatedUserAsync()
        {
            var userId = await _authenticationService.GetAuthenticatedUserIdAsync();
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

        public async Task RemoveAsync(int boardId)
        {
            var boardToRemove = await _context.Boards
                .FirstOrDefaultAsync(board => board.Id == boardId);

            if (await boardBelongToAuthenticatedUserAsync(boardToRemove) == false)
                throw new ArgumentException("Board not exists or user is not author");

            _context.Boards.Remove(boardToRemove!);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> boardBelongToAuthenticatedUserAsync(Board? board)
        {
            var userId = await _authenticationService.GetAuthenticatedUserIdAsync();
            return board?.AuthorId == userId;
        }
    }
}
