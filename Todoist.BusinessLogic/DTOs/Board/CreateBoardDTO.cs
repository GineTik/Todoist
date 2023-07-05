using System.ComponentModel.DataAnnotations;

namespace Todoist.BusinessLogic.DTOs.Board
{
    public class CreateBoardDTO
    {
        [Required]
        public string Name { get; set; }
        public int AuthorId { get; set; }
    }
}
