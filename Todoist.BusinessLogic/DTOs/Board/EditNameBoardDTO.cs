namespace Todoist.BusinessLogic.DTOs.Board
{
    public class EditNameBoardDTO
    {
        public int BoardId { get; set; }
        public string Name { get; set; } = default!;
        public int AuthorId { get; set; }
    }
}
