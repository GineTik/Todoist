using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.Services.Boards
{
    public interface IBoardService
    {
        Task<BoardDTO> CreateAsync(CreateBoardDTO dto);
        Task<ServiceResult> TryRemoveAsync(int boardId);
        Task<ServiceValueResult<BoardDTO>> TryEditNameAsync(EditNameBoardDTO dto);

        Task<IEnumerable<BoardDTO>> GetAllOfAuthenticatedUserAsync();
        Task<BoardWithTasksDTO?> GetAsync(int boardId);

        Task<bool> BoardBelongToAuthenticatedUserAsync(int boardId);
    }
}
