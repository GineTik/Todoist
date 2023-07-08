namespace Todoist.Configuration.Services
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.LoginPath = "Authentication/Login";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["OAuth2.0:Google:ClientId"]!;
                    options.ClientSecret = configuration["OAuth2.0:Google:ClientSecret"]!;
                });

            return services;
        }
    }
}
