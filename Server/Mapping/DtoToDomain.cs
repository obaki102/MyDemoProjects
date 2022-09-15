using AutoMapper;
using MyDemoProjects.Server.Domain.Entities;
using MyDemoProjects.Shared.DTO;

namespace MyDemoProjects.Server.Mapping
{
    public class DtoToDomain : Profile
    {
        public DtoToDomain()
        {
            CreateMap<UserDto, User>()
                .ReverseMap();
        }
        
    }
}
