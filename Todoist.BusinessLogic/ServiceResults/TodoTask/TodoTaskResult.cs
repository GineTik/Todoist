using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.BusinessLogic.ServiceResults.Base;

namespace Todoist.BusinessLogic.ServiceResults.TodoTasks
{
    public sealed class TodoTaskResult : ServiceValueResult<TodoTaskDTO>
    {
        public static ServiceValueResult<TodoTaskDTO> TaskNotBelongUser => Error("TaskNotBelongAuthenticatedUser");
    }

    public sealed class TodoTasksResult : ServiceValueResult<IEnumerable<TodoTaskDTO>>
    {
        public static ServiceValueResult<IEnumerable<TodoTaskDTO>> BoardNotBelongUser => Error("BoardNotBelongAuthenticatedUser");
    }
}
