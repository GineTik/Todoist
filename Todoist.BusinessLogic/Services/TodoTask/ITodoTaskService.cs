using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.Services.TodoTasks
{
    public interface ITodoTaskService
    {
        Task<ServiceValueResult<IEnumerable<TodoTaskDTO>>> TryGetAllAsync(int boardId);
        Task<ServiceValueResult<TodoTaskDTO>> TryCreateAsync(CreateTaskDTO dto);
        Task<ServiceResult> TryRemoveAsync(int taskId);
        Task<ServiceValueResult<TodoTaskDTO>> TryEditAsync(EditTaskDTO dto);
        Task<ServiceValueResult<TodoTaskDTO>> TryToggleClosedAsync(int taskId);
        Task<ServiceValueResult<TodoTaskDTO>> TryEditPositionAsync(EditTaskPositionDTO dto);
    }
}
