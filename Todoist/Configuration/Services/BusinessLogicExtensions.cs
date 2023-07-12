using Todoist.BusinessLogic.Options;
using Todoist.BusinessLogic.Services.Boards;
using Todoist.BusinessLogic.Services.Email;
using Todoist.BusinessLogic.Services.TodoTasks;
using Todoist.BusinessLogic.Services.Users;
using Todoist.BusinessLogic.Services.Users.Authentication;

namespace Todoist.Configuration.Services
{
    public static class BusinessLogicExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
            services.AddTransient<IBoardService, BoardService>();
            services.AddTransient<ITodoTaskService, TodoTaskService>();
            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
