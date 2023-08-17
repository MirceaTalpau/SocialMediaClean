using AutoMapper;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.DTOs;
using SocialMediaClean.APPLICATION.Mapper;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Services;

namespace SocialMediaClean.APPLICATION
{
    public static class ApplicationConfiguration
    {

        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services)
        {
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddScoped<IAuthService,AuthService>();
            return services;
        }
    }
}
