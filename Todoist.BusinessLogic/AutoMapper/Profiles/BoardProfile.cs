using AutoMapper;
using Todoist.BusinessLogic.DTOs.Board;
using Todoist.Data.Models;

namespace Todoist.BusinessLogic.AutoMapper.Profiles
{
    public sealed class BoardProfile : Profile
    {
        public BoardProfile()
        {
            CreateMap<Board, BoardDTO>();
            CreateMap<Board, BoardWithTasksDTO>();
            CreateMap<CreateBoardDTO, Board>();
        }
    }
}
