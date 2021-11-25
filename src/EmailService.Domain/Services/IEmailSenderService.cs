using EmailService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Domain.Services
{
    public interface IEmailSenderService
    {
        Task SendAsync(EmailMessage message);
    }
}
