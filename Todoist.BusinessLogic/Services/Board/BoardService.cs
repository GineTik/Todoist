using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.ServiceResults.Base;
using Todoist.BusinessLogic.ServiceResults.Board;
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
            return boardBelongToAuthenticatedUser(board);
        }

        public async Task<BoardDTO> CreateAsync(CreateBoardDTO dto)
        {
            var newBoard = _mapper.Map<Board>(dto);
            newBoard.AuthorId = _authenticationService.GetAuthenticatedUserId();

            _context.Boards.Add(newBoard);
            await _context.SaveChangesAsync();

            return _mapper.Map<BoardDTO>(newBoard);
        }

        public async Task<ServiceValueResult<BoardDTO>> TryEditNameAsync(EditNameBoardDTO dto)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(board => board.Id == dto.BoardId);

            if (boardBelongToAuthenticatedUser(board) == false)
                return BoardResult.BoardNotBelongUser;

            board!.Name = dto.Name;
            var result = _context.Boards.Update(board);
            await _context.SaveChangesAsync();

            var boardDTO = _mapper.Map<BoardDTO>(result.Entity);
            return BoardResult.Success(boardDTO);
        }

        public async Task<IEnumerable<BoardDTO>> GetAllOfAuthenticatedUserAsync()
        {
            var userId = _authenticationService.GetAuthenticatedUserId();
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

            if (board != null)
                board.Tasks = board.Tasks.OrderBy(board => board.Position);

            return _mapper.Map<BoardWithTasksDTO>(board);
        }

        public async Task<ServiceResult> TryRemoveAsync(int boardId)
        {
            var boardToRemove = await _context.Boards
                .FirstOrDefaultAsync(board => board.Id == boardId);

            if (boardBelongToAuthenticatedUser(boardToRemove) == false)
                return BoardResult.BoardNotBelongUser;

            _context.Boards.Remove(boardToRemove!);
            await _context.SaveChangesAsync();
            return ServiceResult.Success;
        }

        private bool boardBelongToAuthenticatedUser(Board? board)
        {
            var userId = _authenticationService.GetAuthenticatedUserId();
            return board?.AuthorId == userId;
        }
    }
}
