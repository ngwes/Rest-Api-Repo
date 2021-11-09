using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rest_Api_Repo.Domain.Installers;
using Rest_Api_Repo.Domain.Services;
using Rest_Api_Repo.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Installers
{
    public class ServicesInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IPostUriBuilder, PostUriBuilder>();

        }
    }
}
