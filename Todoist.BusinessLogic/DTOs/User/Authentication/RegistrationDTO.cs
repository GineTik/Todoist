using System.ComponentModel.DataAnnotations;

namespace Todoist.BusinessLogic.DTOs.User.Authentication
{
    public class RegistrationDTO : LoginDTO
    {
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = default!;
    }
}
