using AutoMapper;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.BusinessLogic.DTOs.TodoTask;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.AutoMapper.Profiles
{
    public sealed class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, BoardDTO>()
                .ReverseMap();
            CreateMap<Board, CreateBoardDTO>()
                .ReverseMap();
            CreateMap<Board, RemoveBoardDTO>()
                .ReverseMap();
            CreateMap<Board, BoardWithTasksDTO>()
                .ReverseMap();
        }
    }
}
