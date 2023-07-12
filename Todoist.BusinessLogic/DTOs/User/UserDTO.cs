namespace Todoist.BusinessLogic.DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
    }
}
