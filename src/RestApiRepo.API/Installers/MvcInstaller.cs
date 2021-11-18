using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApiRepo.Authorization;
using RestApiRepo.Configurations;
using System;

namespace RestApiRepo.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var apiKeySettings = new ApiKeySettings();
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");
            if (!string.IsNullOrEmpty(apiKey))
                apiKeySettings.Key = apiKey;
            else
                configuration.Bind(nameof(ApiKeySettings), apiKeySettings);

            services.AddSingleton(apiKeySettings);
            services.AddControllers()
                .AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddAuthorization(options =>
            {
                //options.AddPolicy("TagViewer", builder => {
                //    builder.RequireClaim("tags.view","true");
                //});
                options.AddPolicy("WorksForGoogle", options =>
                {
                    options.AddRequirements(new WorksForCompanyRequirement("gmail.com"));
                });
            });
            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();
        }
    }
}
