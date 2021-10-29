using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rest_Api_Repo.Data;
using Rest_Api_Repo.HealthChecks;

namespace Rest_Api_Repo.Installers
{
    public class HealthCheckInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck<CacheHealthCheck>("Cache Check")
                .AddDbContextCheck<DataContext>(); ;
        }
    }
}
