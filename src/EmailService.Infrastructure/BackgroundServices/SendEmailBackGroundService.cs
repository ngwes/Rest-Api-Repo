using EmailService.Domain.Commands;
using EmailService.Domain.Configuration;
using EmailService.Domain.Services;
using MediatR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmailService.Infrastructure.BackgroundServices
{
    public class SendEmailBackGroundService : BackgroundService
    {
        private readonly IMessageQueueService _messageQueueService;
        public SendEmailBackGroundService(IMessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            _messageQueueService.ProcessEvent<SendEmailCommand>();
            return Task.CompletedTask;
        }
    }
}
