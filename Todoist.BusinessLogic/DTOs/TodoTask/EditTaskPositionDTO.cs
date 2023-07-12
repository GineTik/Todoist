namespace Todoist.BusinessLogic.DTOs.TodoTask
{
    public class EditTaskPositionDTO
    {
        public IEnumerable<EditTaskPositionItem> NewPositions { get; set; } = default!;
    }

    public class EditTaskPositionItem
    {
        public int TaskId { get; set; }
        public int NewPosition { get; set; }
    }
}
