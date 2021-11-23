using EmailService.Domain.Configuration;
using EmailService.Domain.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Domain.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly EmailConfiguration _configuration;
        private readonly ISendGridClient _client;

        public EmailSenderService(EmailConfiguration configuration, ISendGridClient client)
        {
            _configuration = configuration;
            _client = client;
        }

        public async Task SendAsync(EmailMessage message)
        {
            var sendGridMessage = new SendGridMessage()
            {
                From = new EmailAddress(_configuration.FromEmail, _configuration.FromName),
                Subject = message.Subject
            };
            sendGridMessage.AddContent(MimeType.Text, message.Body);
            sendGridMessage.AddTos(message.To.Select(to=>new EmailAddress(to)).ToList());
            var response = await _client.SendEmailAsync(sendGridMessage);
        }
    }
}
