using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestApiRepo.Domain.InProcessNotifications
{
    public class UserRegisteredEvent : INotification
    {
        public string UserEmail { get; set; }
        public string UserId { get; set; }
    }
}
