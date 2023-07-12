namespace Todoist.BusinessLogic.DTOs.TodoTask
{
    public class TodoTaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public DateTime DateBeforeExpiration { get; set; }
        public bool IsClosed { get; set; }
        public int Priority { get; set; }
        public int Position { get; set; }
    }
}
