using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Entities
{
    public class EmailMessage
    {
        public IEnumerable<string> To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
