using Todoist.BusinessLogic.DTOs.TodoTask;

namespace Todoist.BusinessLogic.Services.TodoTasks
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTaskDTO>> GetAllAsync(int boardId);
        Task<TodoTaskDTO> CreateAsync(CreateTaskDTO dto);
        Task RemoveAsync(int taskId);
        Task<TodoTaskDTO> EditAsync(EditTaskDTO dto);
    }
}
