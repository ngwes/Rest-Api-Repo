using RestApiRepo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestApiRepo.Domain.Services
{
    public interface IMessageQueueService
    {
        bool SendMessage(IMessageQueueMessage message);
    }
}
