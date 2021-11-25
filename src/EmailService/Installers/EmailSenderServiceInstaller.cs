using EmailService.Domain.Configuration;
using EmailService.Domain.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Api.Installers
{
    public class EmailSenderServiceInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var config = new EmailConfiguration();

            var fromEmail = Environment.GetEnvironmentVariable("FromEmail");
            var fromName = Environment.GetEnvironmentVariable("FromName");
            var sendGridApiKey = Environment.GetEnvironmentVariable("SendGridApiKey") ??
                configuration.GetSection("EmailConfiguration")["ApiKey"];

            if (string.IsNullOrEmpty(fromEmail) ||
                string.IsNullOrEmpty(fromName) ||
                string.IsNullOrEmpty(sendGridApiKey))
            {
                configuration.Bind("EmailConfiguration", config);
            }
            else
            {
               
                config.FromEmail = fromEmail;
                config.FromName = fromName;
            }

            services.AddSingleton(config);
            services.AddSendGrid(options =>
            {
                options.ApiKey = sendGridApiKey;
            });
            services.AddScoped<IEmailSenderService, EmailSenderService>();
        }
    }
}
