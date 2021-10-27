using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rest_Api_Repo.Authorization;
using Rest_Api_Repo.Configurations;
using Rest_Api_Repo.Configurations;
using Rest_Api_Repo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rest_Api_Repo.Installers
{
    public class MvcInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = new JwtSettings();
            var apiKeySettings = new ApiKeySettings();
            configuration.Bind(nameof(ApiKeySettings), apiKeySettings);
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
            services.AddSingleton(apiKeySettings);
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddControllers()
                .AddJsonOptions(options=>
                options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services
                .AddFluentValidation(config=>config.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            var tokenAuthenticationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true

            };
            services.AddSingleton(tokenAuthenticationParameters);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x=> {
                    x.SaveToken = true;
                    x.TokenValidationParameters = tokenAuthenticationParameters;
                });
            services.AddAuthorization(options=> {
                //options.AddPolicy("TagViewer", builder => {
                //    builder.RequireClaim("tags.view","true");
                //});
                options.AddPolicy("WorksForGoogle", options => {
                    options.AddRequirements(new WorksForCompanyRequirement("gmail.com"));
                });
            });
            services.AddSingleton<IAuthorizationHandler, WorksForCompanyHandler>();
            services.AddSingleton<IUriService, UriService>();
        }
    }
}
