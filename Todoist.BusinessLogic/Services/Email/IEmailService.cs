namespace Todoist.BusinessLogic.Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
        Task SendConfirmationEmailAsync(string email, string code);
    }
}
