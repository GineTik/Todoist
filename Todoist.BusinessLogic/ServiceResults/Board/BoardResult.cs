using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.ServiceResults.Board
{
    public sealed class BoardResult : ServiceValueResult<BoardDTO>
    {
        public static ServiceValueResult<BoardDTO> BoardNotBelongUser => Error("BoardNotBelongAuthenticatedUser");
    }
}
