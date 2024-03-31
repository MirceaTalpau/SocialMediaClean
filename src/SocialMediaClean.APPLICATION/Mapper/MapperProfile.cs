using AutoMapper;
using LinkedFit.DOMAIN.Models.DTOs.Auth;
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
