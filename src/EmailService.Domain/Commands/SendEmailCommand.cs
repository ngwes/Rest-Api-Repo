using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Commands
{
    public class SendEmailCommand : IRequest
    {
        public IEnumerable<string> To{ get; set; }
        public string Body{ get; set; }
        public string Subject{ get; set; }
    }
}
