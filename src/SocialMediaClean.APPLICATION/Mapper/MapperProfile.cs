using AutoMapper;
using SocialMediaClean.APPLICATION.DTOs;
using SocialMediaClean.APPLICATION.Requests;

namespace SocialMediaClean.APPLICATION.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile() 
        {
            CreateMap<RegisterRequest, RegisterRequestDTO>();
        }

    }
}
