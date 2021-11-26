using MediatR;
using RestApiRepo.Domain.Entities;
using RestApiRepo.Domain.InProcessNotifications;
using RestApiRepo.Domain.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Handlers.V1.Comments
{
    public class EmailSenderForUserRegisteredEvent : INotificationHandler<UserRegisteredEvent>
    {
        private readonly IEmailService _emailService;

        public EmailSenderForUserRegisteredEvent(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _emailService.SendEmailMessage(new EmailMessage
            {
                To = new List<string> { notification.UserEmail },
                Subject = "Successfull Registration",
                Body = "Your account has been successfully registered"
            });
            return Task.CompletedTask;
        }
    }
}
