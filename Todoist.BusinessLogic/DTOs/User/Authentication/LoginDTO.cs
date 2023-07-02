using System.ComponentModel.DataAnnotations;

namespace Todoist.BusinessLogic.DTOs.User.Authentication
{
    public class LoginDTO
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = default!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;
    }
}
