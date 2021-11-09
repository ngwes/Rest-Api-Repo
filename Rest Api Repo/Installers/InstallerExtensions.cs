using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Domain.Installers;
using System;
using System.Linq;

namespace Rest_Api_Repo.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>()
                .ToList();
            installers.ForEach(installer => installer.InstallServices(services, configuration, env));
        }
    }
}
