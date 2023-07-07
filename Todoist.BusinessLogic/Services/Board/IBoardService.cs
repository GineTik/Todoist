using Todoist.BusinessLogic.DTOs.Board;

namespace Todoist.BusinessLogic.Services.Boards
{
    public interface IBoardService
    {
        Task<BoardDTO> CreateAsync(CreateBoardDTO dto);
        Task RemoveAsync(RemoveBoardDTO dto);
        Task<IEnumerable<BoardDTO>> GetAllAsync(int userId);
        Task<BoardWithTasksDTO?> GetAsync(int boardId);
        Task<BoardDTO> EditNameAsync(EditNameBoardDTO dto);
        Task<bool> BoardBelongToUserAsync(BoardBelongToUserDTO dto);
    }
}
