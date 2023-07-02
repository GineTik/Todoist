using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Todoist.Data.Models.Base;

namespace Todoist.Data.Models
{
    public class Board : BaseModel
    {
        [Required]
        public string Name { get; set; } = default!;

        [ForeignKey(nameof(Author))]
        public int AuthorId{ get; set; }
        public User? Author { get; set; }

        public IEnumerable<TodoTask> Tasks { get; set; } = default!;
    }
}
