using LinkedFit.INFRASTRUCTURE.Implementation;
using LinkedFit.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Implementation;
using SocialMediaClean.INFRASTRUCTURE.Interfaces;
using SocialMediaClean.INFRASTRUCTURE.Settings;

namespace SocialMediaClean.INFRASTRUCTURE
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructureConfiguration(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddScoped<IMailService, MailService>();
            services.AddScoped<IUploadFilesService, UploadFilesService>();
            return services;
        }
    }
}
    
