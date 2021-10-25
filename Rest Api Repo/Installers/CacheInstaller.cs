using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Configurations;
using Rest_Api_Repo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisSettings = new RedisConfiguration();
            configuration.Bind(nameof(RedisConfiguration), redisSettings);

            services.AddSingleton(redisSettings);
            if (!redisSettings.Enabled)
            {
                return;
            }

            services.AddStackExchangeRedisCache(options=> {
                options.Configuration = redisSettings.ConnectionString;
            });

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

        }
    }
}
