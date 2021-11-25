using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmailService.Domain.Services
{
    public interface IMessageQueueService
    {
        void ProcessEvent<T>();
        void PreEventProcessing(params string[] args);
        void PostEventProcessing(params string[] args);
    }
}
