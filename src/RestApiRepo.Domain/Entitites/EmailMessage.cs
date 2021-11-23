
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.Entities
{
    public class EmailMessage : IMessageQueueMessage
    {
        public IEnumerable<string> To { get; set; }
        public string Body { get; set; }
        public string Subject { get; set; }
    }
}
