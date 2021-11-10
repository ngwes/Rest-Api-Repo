using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Infrastructure;
using Rest_Api_Repo.Domain.Installers;
using Rest_Api_Repo.HealthChecks;

namespace Rest_Api_Repo.Installers
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
