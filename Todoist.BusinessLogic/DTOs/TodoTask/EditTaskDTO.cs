namespace Todoist.BusinessLogic.DTOs.TodoTask
{
    public class EditTaskDTO
    {
        public int TaskId { get; set; }
        public int AuthorId { get; set; }
        public string Title { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime ClosingDate { get; set; }
    }
}
