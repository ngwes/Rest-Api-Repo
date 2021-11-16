using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiRepo.HealthChecks;
using RestApiRepo.Infrastructure;

namespace RestApiRepo.Installers
{
    public class HealthCheckInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddHealthChecks()
                .AddCheck<CacheHealthCheck>("Cache Check")
                .AddDbContextCheck<DataContext>(); ;
        }
    }
}
