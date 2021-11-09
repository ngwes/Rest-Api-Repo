using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Rest_Api_Repo.Domain.Installers;
using System;
using System.IO;
using System.Reflection;

namespace Rest_Api_Repo.Installers
{
    public class SwaggerInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MyRestRepoApi",
                    Version = "v1"
                });
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };
                c.AddSecurityDefinition("Bearer", securitySchema);
                var securityRequirement = new OpenApiSecurityRequirement
                {
                    { securitySchema, new[] { "Bearer" } }
                };
                c.AddSecurityRequirement(securityRequirement);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
