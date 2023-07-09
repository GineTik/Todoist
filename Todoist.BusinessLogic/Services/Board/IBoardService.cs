using Todoist.BusinessLogic.DTOs.Board;

namespace Todoist.BusinessLogic.Services.Boards
{
    public interface IBoardService
    {
        Task<BoardDTO> CreateAsync(CreateBoardDTO dto);
        Task RemoveAsync(int boardId);
        Task<BoardDTO> EditNameAsync(EditNameBoardDTO dto);

        Task<IEnumerable<BoardDTO>> GetAllOfAuthenticatedUserAsync();
        Task<BoardWithTasksDTO?> GetAsync(int boardId);

        Task<bool> BoardBelongToAuthenticatedUserAsync(int boardId);
    }
}
