using Microsoft.AspNetCore.Identity;
using Todoist.Data.EF;
using Todoist.Data.Models;

namespace Todoist.Configuration.Services
{
    public static class ConfigureIdentityExtension
    {
        public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;

            }).AddEntityFrameworkStores<DataContext>();

            return services;
        }
    }
}
