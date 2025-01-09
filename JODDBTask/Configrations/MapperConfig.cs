using AutoMapper;
using JODDBTask.Core.Data;
using JODDBTask.Core.Dto;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace JODDBTask.Api.Configrations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User,CreateUserDto>().ReverseMap();
        }
    }
}
