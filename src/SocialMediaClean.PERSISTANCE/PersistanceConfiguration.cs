using LinkedFit.PERSISTANCE.Interfaces;
using LinkedFit.PERSISTANCE.Repositories;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.PERSISTANCE.Interfaces;
using SocialMediaClean.PERSISTANCE.Repositories;

namespace SocialMediaClean.PERSISTANCE
{
    public static class PersistanceConfiguration
    {
        public static IServiceCollection AddPersistanceConfiguration(this IServiceCollection services)
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            ////////POSTS
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostActionsRepository, PostActionsRepository>();
            ////////COMMENTS
            services.AddScoped<ICommentRepository, CommentRepository>();
            ////////FRIENDS
            services.AddScoped<IFriendsRepository, FriendsRepository>();
            ////////CHAT
            services.AddScoped<IChatRepository, ChatRepository>();
            ///////FEED
            services.AddScoped<IFeedRepository, FeedRepository>();

            return services;
        }
    }
}
