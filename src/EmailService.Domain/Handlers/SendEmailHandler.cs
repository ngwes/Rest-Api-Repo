using EmailService.Domain.Commands;
using EmailService.Domain.Entities;
using EmailService.Domain.Services;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmailService.Domain.Handlers
{
    public class SendEmailHandler : AsyncRequestHandler<SendEmailCommand>
    {
        private readonly IEmailSenderService _emailSenderService;

        public SendEmailHandler(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        protected override async Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var message = new EmailMessage
            {
                Body = request.Body,
                Subject = request.Subject,
                To = request.To
            };
            try
            {
                await _emailSenderService.SendAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            Console.WriteLine(request.Body);
            Console.WriteLine(request.To.ToList());
        }
    }
}
