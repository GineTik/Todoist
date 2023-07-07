using Todoist.BusinessLogic.DTOs.TodoTask;

namespace Todoist.BusinessLogic.Services.TodoTasks
{
    public interface ITodoTaskService
    {
        Task<IEnumerable<TodoTaskDTO>> GetAllAsync(GetTaskDTO dto);
        Task<TodoTaskDTO> CreateAsync(CreateTaskDTO dto);
        Task RemoveAsync(RemoveTaskDTO dto);
        Task<TodoTaskDTO> EditAsync(EditTaskDTO dto);
    }
}
