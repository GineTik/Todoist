using Todoist.BusinessLogic.Options;

namespace Todoist.Configuration.Services
{
    public static class OptionsExtensions
    {
        public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailOptions>(configuration.GetSection("EmailOptions"));

            return services;
        }
    }
}
