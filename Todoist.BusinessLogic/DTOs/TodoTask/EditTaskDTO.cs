namespace Todoist.BusinessLogic.DTOs.TodoTask
{
    public class EditTaskDTO
    {
        public int TaskId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime DateBeforeExpiration { get; set; }
    }
}
