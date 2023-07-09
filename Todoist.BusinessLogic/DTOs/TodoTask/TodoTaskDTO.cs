namespace Todoist.BusinessLogic.DTOs.TodoTask
{
    public class TodoTaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public DateTime ClosingDate { get; set; }
        public int Priority { get; set; }
    }
}
