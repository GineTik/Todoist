using AutoMapper;
using Todoist.BusinessLogic.DTOs.Page;
using Todoist.Data.Models.Page;

namespace Todoist.BusinessLogic.AutoMapper.Profiles
{
    public class PageProfile : Profile
    {
        public PageProfile()
        {
            CreateMap(typeof(Page<>), typeof(PageDTO<>));
        }
    }
}
