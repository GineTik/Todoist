using Microsoft.AspNetCore.Identity;
using Todoist.Data.Models.Base;

namespace Todoist.Data.Models
{
    public class User : IdentityUser<int>, IBaseModel
    {
        public IEnumerable<Board> Boards { get; set; } = default!;
    }
}
