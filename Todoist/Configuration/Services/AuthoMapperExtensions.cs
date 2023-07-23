using Todoist.BusinessLogic.AutoMapper.Profiles;

namespace Todoist.Configuration.Services
{
    public static class AuthoMapperExtensions
    {
        public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
        {
            return services.AddAutoMapper(
                typeof(BoardProfile), 
                typeof(TodoTaskProfile), 
                typeof(PageProfile)
            );
        }
    }
}
