namespace Todoist.BusinessLogic.DTOs.User.Authentication
{
    public class ConfirmEmailDTO
    {
        public string Email { get; set; } = default!;
        public string Code { get; set; } = default!;
    }
}
