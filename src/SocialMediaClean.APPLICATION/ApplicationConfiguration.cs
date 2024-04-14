using AutoMapper;
using LinkedFit.APPLICATION.Contracts;
using LinkedFit.APPLICATION.Services;
using SocialMediaClean.APPLICATION.Contracts;
using SocialMediaClean.APPLICATION.Logger;
using SocialMediaClean.APPLICATION.Mapper;
using SocialMediaClean.APPLICATION.Requests;
using SocialMediaClean.APPLICATION.Services;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;

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
            services.AddScoped<IAccountService,AccountService>();
            services.AddSingleton<ILoggerProvider>(sp =>
            {
                var connectionFactory = sp.GetRequiredService<IDbConnectionFactory>();
                return new DbLoggerProvider(connectionFactory);
            });

            ////////////////POSTS/////////////////////
            services.AddScoped<IPostService, PostService>();

            ////////////////FEED/////////////////////
            services.AddScoped<IFeedService, FeedService>();

            return services;

        }
    }
}
