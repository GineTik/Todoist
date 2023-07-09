using System.ComponentModel.DataAnnotations;

namespace Todoist.BusinessLogic.DTOs.TodoTask
{
    public class CreateTaskDTO
    {
        [Required]
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        [Required]
        public DateTime ClosingDate { get; set; }
        public int BoardId { get; set; }
    }
}
