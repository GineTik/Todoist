using Todoist.BusinessLogic.DTOs.Page;

namespace Todoist.BusinessLogic.DTOs.Board
{
    public class GetBoardWithTasksDTO
    {
        public int BoardId { get; set; }
        public PageInfo PageInfo { get; set; } = default!;
    }
}
