using AutoMapper;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.AutoMapper.Profiles
{
    public sealed class TodoTaskProfile : Profile
    {
        public TodoTaskProfile()
        {
            CreateMap<TodoTask, TodoTaskDTO>()
                .ReverseMap();
            CreateMap<TodoTask, GetTaskDTO>()
                .ReverseMap();
            CreateMap<TodoTask, CreateTaskDTO>()
                .ReverseMap();
            CreateMap<TodoTask, RemoveTaskDTO>()
                .ReverseMap();
        }
    }
}
