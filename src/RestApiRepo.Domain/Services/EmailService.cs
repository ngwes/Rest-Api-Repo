using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly IMessageQueueService _messageQueueService;

        public EmailService(IMessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
        }

        public bool SendEmailMessage(EmailMessage message)
        {
            return _messageQueueService.SendMessage(message);
        }
    }
}
