using Todoist.BusinessLogic.DTOs.TodoTask;

namespace Todoist.BusinessLogic.DTOs.Board
{
    public class BoardWithTasksDTO : BoardDTO
    {
        public BoardWithTasksDTO()
        {
            Tasks = new List<TodoTaskDTO>();
        }

        public IEnumerable<TodoTaskDTO> Tasks { get; } = default!;
    }
}
