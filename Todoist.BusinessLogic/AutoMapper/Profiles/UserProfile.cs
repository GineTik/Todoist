using AutoMapper;
using Todoist.BusinessLogic.DTOs.User;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.AutoMapper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
