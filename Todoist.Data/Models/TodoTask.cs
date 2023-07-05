using System.ComponentModel.DataAnnotations;
using Todoist.Data.Models.Base;

namespace Todoist.Data.Models
{
    public class TodoTask : BaseModel
    {
        [Required]
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        [Required]
        public DateTime ClosingDate { get; set; }
        public int Priority { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }
    }
}
