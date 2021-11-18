using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiRepo.Cache;
using RestApiRepo.Configurations;
using System;

namespace RestApiRepo.Installers
{
    public class CacheInstaller : IInstaller
    {

        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var redisSettings = new CacheConfiguration();
            var isCacheEnabled = Environment.GetEnvironmentVariable("CacheEnabled");
            var cacheConnectionString = Environment.GetEnvironmentVariable("CacheConnectionString");
            
            if(!string.IsNullOrEmpty(isCacheEnabled) && !string.IsNullOrEmpty(cacheConnectionString))
            {
                redisSettings.Enabled = bool.Parse(isCacheEnabled);
                redisSettings.ConnectionString = cacheConnectionString;
            }
            else
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
