using EmailService.Infrastructure.BackgroundServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.Api.Installers
{
    public class HostedServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddHostedService<SendEmailBackGroundService>();
        }
    }
}
