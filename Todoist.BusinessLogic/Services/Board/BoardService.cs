using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.DTOs.Page;
using Todoist.BusinessLogic.ServiceResults.Base;
using Todoist.BusinessLogic.ServiceResults.Board;
using Todoist.BusinessLogic.Services.Users.Authentication;
using Todoist.Data.EF;
using Todoist.Data.EF.Extensions;
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

        public async Task<PageDTO<BoardDTO>> GetPageOfAuthenticatedUserAsync(PageInfo info)
        {
            var userId = _authenticationService.GetAuthenticatedUserId();
            var page = await _context.Boards
                .Where(board => board.AuthorId == userId)
                .PaginateAsync(info.Page, info.Size);

            return _mapper.Map<PageDTO<BoardDTO>>(page);
        }

        public async Task<BoardWithTasksDTO?> GetWithTasksAsync(int boardId)
        {
            var board = await _context.Boards
                .Include(board => board.Tasks.OrderBy(task => task.Position))
                .FirstOrDefaultAsync(board => board.Id == boardId);

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
