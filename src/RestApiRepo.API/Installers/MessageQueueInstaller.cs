using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RestApiRepo.Domain.Configurations;
using RestApiRepo.Domain.Services;
using RestApiRepo.Infrastructure.Services;
using System;

namespace RestApiRepo.Installers
{
    public class MessageQueueInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            var config = new MessageQueueConfiguration();

            var queueHostname = Environment.GetEnvironmentVariable("QueueHostName");
            var queueUser = Environment.GetEnvironmentVariable("QueueUser");
            var queuePassword = Environment.GetEnvironmentVariable("QueuePassword");
            var queueName = Environment.GetEnvironmentVariable("QueueName");
            var emailQueueName = Environment.GetEnvironmentVariable("EmailQueueName");
            if (string.IsNullOrEmpty(queueHostname) ||
                string.IsNullOrEmpty(queueUser) ||
                string.IsNullOrEmpty(queuePassword) ||
                string.IsNullOrEmpty(emailQueueName) ||
                string.IsNullOrEmpty(queueName))
            {
                configuration.Bind("MessageQueue", config);
            }
            else
            {
                config.HostName = queueHostname;
                config.Password = queuePassword;
                config.User = queueUser;
                config.QueueName = queueName;
                config.EmailQueueName = emailQueueName;
            }

            services.AddSingleton(config);

            var factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.User,
                Password = config.Password,
            };

            services.AddSingleton(factory);
            services.AddScoped<IMessageQueueService, MessageQueueService>();

        }
    }
}
