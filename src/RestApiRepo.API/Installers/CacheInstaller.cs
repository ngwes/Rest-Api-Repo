using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiRepo.Cache;
using RestApiRepo.Configurations;

namespace RestApiRepo.Installers
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
