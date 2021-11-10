using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Domain.Installers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RestApiRepo.Installers
{
    public class ValidationInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services
               .AddFluentValidation(config => config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
