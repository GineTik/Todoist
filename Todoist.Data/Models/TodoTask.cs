using System.ComponentModel.DataAnnotations;
using Todoist.Data.Models.Base;

namespace Todoist.Data.Models
{
    public class TodoTask : BaseModel
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        [Required]
        public DateTime DateBeforeExpiration { get; set; }
        public bool IsClosed { get; set; }
        public int Priority { get; set; }
        public int Position { get; set; }

        public int BoardId { get; set; }
        public Board? Board { get; set; }
    }
}
