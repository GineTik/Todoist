using Microsoft.EntityFrameworkCore;
using Todoist.Data.EF;

namespace Todoist.BusinessLogic.Services.Users
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;

        public UserService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(user => user.Email == email);
        }
    }
}
