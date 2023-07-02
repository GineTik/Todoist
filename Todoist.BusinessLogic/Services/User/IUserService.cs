namespace Todoist.BusinessLogic.Services.Users
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string email);
    }
}
