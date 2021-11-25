using System;
using System.Collections.Generic;
using System.Text;

namespace EmailService.Domain.Configuration
{
    public class MessageQueueConfiguration
    {
        public string HostName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
    }
}
