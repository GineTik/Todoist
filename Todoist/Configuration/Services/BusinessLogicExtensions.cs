using Todoist.BusinessLogic.Services.Boards;
using Todoist.BusinessLogic.Services.Users.Authentication;
using Todoist.BusinessLogic.Services.Users;
using Todoist.BusinessLogic.Services.TodoTasks;

namespace Todoist.Configuration.Services
{
    public static class BusinessLogicExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserAuthenticationService, UserAuthenticationService>();
            services.AddTransient<IBoardService, BoardService>();
            services.AddTransient<ITodoTaskService, TodoTaskService>();

            return services;
        }
    }
}
