using Microsoft.EntityFrameworkCore;
using Todoist.Configuration.Services;
using Todoist.Data.EF;

namespace Todoist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection")));

            builder.Services.AddMvc();

            builder.Services.ConfigureIdentity();

            var app = builder.Build();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}