using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Cache;
using Rest_Api_Repo.Configurations;
using Rest_Api_Repo.Domain.Installers;

namespace Rest_Api_Repo.Installers
{
    public class CacheInstaller : IInstaller
    {

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var redisSettings = new CacheConfiguration();
            configuration.Bind(nameof(CacheConfiguration), redisSettings);
            services.AddSingleton(redisSettings);

            if (!redisSettings.Enabled)
            {
                return;
            }
            if (env.EnvironmentName.Equals("Testing"))
            {
                services.AddDistributedMemoryCache();
            }
            else
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisSettings.ConnectionString;
                });
            }

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

        }
    }
}
